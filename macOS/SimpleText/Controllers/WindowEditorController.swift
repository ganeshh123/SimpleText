//
//  WindowEditorController.swift
//  SimpleText
//
//  Created by Ganesh on 1/19/23.
//

import Cocoa

class WindowEditorController: NSWindowController, NSWindowDelegate, NSTextViewDelegate {
    
    private let windowId: UUID = UUID()
    private var openedFile: URL? = nil
    private var openedFileInitialText: String = ""
    private var fileModified: Bool = false
    private var darkModeEnabled: Bool = false
    @IBOutlet var textViewEditor: NSTextView!
    
    @IBOutlet var darkThemeMenuItem: NSMenuItem!
    @IBOutlet var wordWrapMenuItem: NSMenuItem!
    
    convenience init() {
        self.init(windowNibName: "WindowEditor")
    }

    override func windowDidLoad() {
        super.windowDidLoad()
        self.window?.title = "New File - Simple Text"
        
        guard let appDelegate = NSApplication.shared.delegate as? AppDelegate else {
             return
        }
        if(appDelegate.darkModeEnabled){
            darkThemeMenuItem.isEnabled = true
            darkThemeMenuItem.state = .on
        }
        setWordWrap(enabled: appDelegate.wordWrapEnabled)
        wordWrapMenuItem.state = appDelegate.wordWrapEnabled ? .on : .off
        
        setTextFont(newFont: appDelegate.editorFont!)
        
        textViewEditor.delegate = self
    }
    
    /* Get/Set Functions */
    
    func getWindowId() -> UUID {
        return windowId
    }
    
    func getFileModified() -> Bool{
        return fileModified
    }
    
    
    /* Window Events */
    
    func windowShouldClose(_ sender: NSWindow) -> Bool {
        if(fileModified == true){
            let fileName: String = openedFile?.lastPathComponent ?? "New File"
            
            let saveFileCheck = NSAlert()
            saveFileCheck.messageText = "Would you like to save changes to \(fileName)?"
            saveFileCheck.addButton(withTitle: "Yes")
            saveFileCheck.addButton(withTitle: "No")
            saveFileCheck.alertStyle = .informational
            
            let saveRequested: Bool = saveFileCheck.runModal() == .alertFirstButtonReturn
            if(saveRequested){
                saveMenuItemClicked(nil)
                self.close()
            }
            
        }
        return true
    }
    
    func windowWillClose(_ notification: Notification) {
        guard let appDelegate = NSApplication.shared.delegate as? AppDelegate else {
             return
        }
        appDelegate.removeWindowEditor(windowId: windowId)
    }
    
    /* File Read/Write Functions */
    
    func readFile(file: URL){
        var fileContent = ""
        do{
            fileContent = try String(contentsOf: file)
        }
        catch{
            assertionFailure("Could not read \(file.path): \(error)")
        }
        
        openedFile = file
        openedFileInitialText = fileContent
        self.window?.title = "\(file.lastPathComponent) - SimpleText"
        fileModified = false
        
        textViewEditor.string = fileContent
        updateFileModified()
    }
    
    func writeFile(file: URL){
        let fileContent = textViewEditor.string
        
        do{
            try fileContent.write(to: file, atomically: true, encoding: String.Encoding.utf8)
        }catch{
            assertionFailure("An error occured saving the file \(file.path) : \(error)")
        }
    }
    
    func updateFileModified(){
        fileModified = textViewEditor.string != openedFileInitialText
        if(fileModified == true){
            if(openedFile != nil){
                self.window?.title = "\(openedFile!.lastPathComponent)* - SimpleText"
            }
        }else if(((self.window?.title.contains("*")) != nil) && openedFile != nil){
            self.window?.title = "\(openedFile!.lastPathComponent) - SimpleText"
        }
    }
    
    func setWordWrap(enabled: Bool){
        if(enabled){
            textViewEditor.textContainer?.widthTracksTextView = true
            let restoreSize: CGSize = textViewEditor.enclosingScrollView!.contentSize
            textViewEditor.textContainer?.containerSize = CGSize(width: restoreSize.width, height: (textViewEditor.textContainer?.containerSize.height)!)
            
        }else{
            textViewEditor.maxSize = CGSize(width: CGFloat.greatestFiniteMagnitude, height: CGFloat.greatestFiniteMagnitude)
            textViewEditor.isHorizontallyResizable = true
            textViewEditor.textContainer?.widthTracksTextView = false
            textViewEditor.textContainer?.containerSize = CGSize(width: CGFloat.greatestFiniteMagnitude, height: (textViewEditor.textContainer?.containerSize.height)!)
        }
    }
    
    func setTextFont(newFont: NSFont){
        textViewEditor?.font = newFont
    }
    
    // NSTextViewDelegate conforms to NSTextDelegate
    func textDidChange(_ notification: Notification) {
        updateFileModified()
    }
    
    /* Menu Button Events */
    
    @IBAction func newMenuItemClicked(_ sender: NSMenuItem) {
        guard let appDelegate = NSApplication.shared.delegate as? AppDelegate else {
             return
        }
        appDelegate.createWindowEditor()
    }
    
    
    @IBAction func openMenuItemClicked(_ sender: NSMenuItem) {
        let openFile: NSOpenPanel = NSOpenPanel()
        
        openFile.showsHiddenFiles = true
        openFile.canChooseDirectories = false
        openFile.allowsMultipleSelection = false
        
        if(openFile.runModal() != NSApplication.ModalResponse.OK){
            return
        }
        
        let result = openFile.url
        if(result == nil){
            return
        }
        
        if(openedFile != nil || fileModified == true){
            guard let appDelegate = NSApplication.shared.delegate as? AppDelegate else {
                return
            }
            appDelegate.createWindowEditor(openFile: result)
        }else{
            readFile(file: result!)
        }
        
    }
    
    @IBAction func saveMenuItemClicked(_ sender: NSMenuItem?) {
        if(openedFile == nil){
            saveAsMenuItemClicked(sender)
            return
        }
        
        writeFile(file: openedFile!)
        readFile(file: openedFile!)
    }
    
    @IBAction func saveAsMenuItemClicked(_ sender: NSMenuItem?) {
        let saveFile: NSSavePanel = NSSavePanel()
        
        saveFile.allowedFileTypes = ["txt"]
        
        saveFile.showsHiddenFiles = true
        saveFile.allowsOtherFileTypes = true
        saveFile.isExtensionHidden = false
        
        if(saveFile.runModal() != NSApplication.ModalResponse.OK){
            return
        }
        
        let result = saveFile.url
        if(result == nil){
            return
        }
        
        writeFile(file: result!)
        readFile(file: result!)
    }
    
    @IBAction func fontMenuItemClicked(_ sender: NSMenuItem) {
        guard let appDelegate = NSApplication.shared.delegate as? AppDelegate else {
             return
        }
        appDelegate.showFontPanel(currentFont: textViewEditor.font)
    }
    
    
    @IBAction func wordWrapMenuItemClicked(_ sender: NSMenuItem) {
        guard let appDelegate = NSApplication.shared.delegate as? AppDelegate else {
             return
        }
        let enableWordWrap: Bool =  !appDelegate.wordWrapEnabled
        appDelegate.setEditorWordWrap(enabled: enableWordWrap)
        
        if(enableWordWrap){
            sender.state = .on
        }else{
            sender.state = .off
        }
    }
    
    
    @IBAction func zoomInMenuItemClicked(_ sender: NSMenuItem) {
        textViewEditor.font = NSFont(name: textViewEditor.font!.fontName, size: textViewEditor.font!.pointSize + 1)
    }
    
    @IBAction func zoomOutMenuItemClicked(_ sender: Any) {
        textViewEditor.font = NSFont(name: textViewEditor.font!.fontName, size: textViewEditor.font!.pointSize - 1)
    }
    
    @IBAction func resetZoomMenuItemClicked(_ sender: Any) {
        guard let appDelegate = NSApplication.shared.delegate as? AppDelegate else {
             return
        }
        textViewEditor.font = NSFont(name: textViewEditor.font!.fontName, size: appDelegate.editorFont!.pointSize)
    }
    
    @IBAction func darkThemeMenuItemClicked(_ sender: NSMenuItem) {
        let enableDarkTheme: Bool = !(sender.state == .on)
        if(enableDarkTheme){
            if #available(macOS 10.14, *) {
                NSApp.appearance = NSAppearance(named: .darkAqua)
            }
            sender.state = .on
        }else{
            if #available(macOS 10.14, *) {
                NSApp.appearance = NSAppearance(named: .aqua)
            }
            sender.state = .off
        }
    }
    
    @IBAction func viewHelpMenuItemClicked(_ sender: NSMenuItem) {
        NSWorkspace.shared.open(NSURL(string: "https://ganeshh123.github.io/SimpleText/#usage")! as URL)
    }
    
    
    
    @IBAction func licenseMenuItemClicked(_ sender: NSMenuItem) {
        NSWorkspace.shared.open(NSURL(string: "https://github.com/ganeshh123/SimpleText/blob/main/LICENSE.MD")! as URL)
    }
    
    @IBAction func checkForUpdatesmenuItemClicked(_ sender: NSMenuItem) {
        NSWorkspace.shared.open(NSURL(string: "https://github.com/ganeshh123/SimpleText/releases")! as URL)
    }
    
    @IBAction func reportBugMenuItemClicked(_ sender: NSMenuItem) {
        NSWorkspace.shared.open(NSURL(string: "https://github.com/ganeshh123/SimpleText/issues")! as URL)
    }
    
    
}
