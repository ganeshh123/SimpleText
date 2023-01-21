//
//  WindowEditorController.swift
//  SimpleText
//
//  Created by Ganesh on 1/19/23.
//

import Cocoa

class WindowEditorController: NSWindowController, NSWindowDelegate {
    
    private let windowId: UUID = UUID()
    private let defaultFont: NSFont? = NSFont(name: "Courier New", size: 18)
    private var openedFile: URL? = nil
    private var openedFileInitialText: String = ""
    private var fileModified: Bool = false
    
    
    
    @IBOutlet var textViewEditor: NSTextView!
    
    
    convenience init() {
        self.init(windowNibName: "WindowEditorController")
    }

    override func windowDidLoad() {
        super.windowDidLoad()

        textViewEditor.font = defaultFont
    }
    
    /* Get/Set Functions */
    
    func getWindowId() -> UUID {
        return windowId
    }
    
    
    /* Window Events */
    
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
            assertionFailure("Could not read \(file.path)")
        }
        
        openedFile = file
        openedFileInitialText = fileContent
        self.window?.title = "\(file.lastPathComponent) - SimpleText"
        fileModified = false
        
        textViewEditor.string = fileContent
    }
    
    func writeFile(file: URL){
        let fileContent = textViewEditor.string
        
        do{
            try fileContent.write(to: file, atomically: true, encoding: String.Encoding.utf8)
        }catch{
            assertionFailure("An error occured saving the file \(file.path) : \(error)")
        }
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
        
        readFile(file: result!)
    }
    
    @IBAction func saveMenuItemClicked(_ sender: NSMenuItem) {
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
    
}
