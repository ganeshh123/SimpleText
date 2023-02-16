//
//  AppDelegate.swift
//  SimpleText
//
//  Created by Ganesh on 1/19/23.
//

import Cocoa

@main
class AppDelegate: NSObject, NSApplicationDelegate {
    
    let fileManager = FileManager.default
    var appConfig: IniFile? = nil
    
    var openWindows = [String : WindowEditorController]()
    var quitRequested: Bool = false
    // Application level settings
    var darkModeEnabled: Bool = false
    var wordWrapEnabled: Bool = true
    var editorFont: NSFont? = NSFont(name: "Courier New", size: 18)
    
    
    func applicationDidFinishLaunching(_ aNotification: Notification) {
        // Remove annoying forced help menu customization
        let unusedMenu: NSMenu = NSMenu(title: "Help")
        NSApplication.shared.helpMenu = unusedMenu
        
        if #available(macOS 10.14, *){
            darkModeEnabled = UserDefaults.standard.string(forKey: "AppleInterfaceStyle") == "Dark"
        }
        
        loadSettings()
        
        createWindowEditor()
    }
    
    func applicationShouldTerminate(_ sender: NSApplication) -> NSApplication.TerminateReply {
        quitRequested = true
        for wE in openWindows.values{
            if(wE.getFileModified() == true){
                closeAllWindows()
                return .terminateCancel
            }
        }
        return .terminateNow
    }

    func applicationWillTerminate(_ aNotification: Notification) {
        // Insert code here to tear down your application
    }

    func applicationSupportsSecureRestorableState(_ app: NSApplication) -> Bool {
        return true
    }
    
    
    func loadSettings(){
        do{
            let userDataFolder: URL? = fileManager.urls(for: .applicationSupportDirectory, in: .userDomainMask).first?.appendingPathComponent("SimpleText")
            let configFilePath: URL? = userDataFolder?.appendingPathComponent("SimpleTextSettings.ini")
            if(configFilePath != nil && fileManager.fileExists(atPath: configFilePath!.path)){
                
                appConfig = IniFile(iniPath: configFilePath!.path)
                
                if(appConfig == nil){
                    return
                }
                
                if(appConfig!.read(queryKey: "wordWrapEnabled") != nil){
                    let valueStr: String = appConfig!.read(queryKey: "wordWrapEnabled")!
                    wordWrapEnabled = valueStr == "true" || (valueStr != "false" && wordWrapEnabled)
                }
                if(appConfig!.read(queryKey: "editorFont") != nil){
                    let valueStr: String = appConfig!.read(queryKey: "editorFont")!
                    let fontComponents: [String] = valueStr.components(separatedBy: ", ")
                    
                    var fontName: String = "Courier New"
                    var fontSize: Float = 18
                    
                    if(fontComponents.count == 2){
                        fontName = fontComponents[0]
                        fontSize = (fontComponents[1] as NSString).floatValue
                    }
                    
                    let newFont: NSFont? = NSFont(name: fontName, size: CGFloat(fontSize))
                    
                    editorFont = newFont != nil ? newFont : NSFont(name: "Courier New", size: 18)!
                }
                
            }else{
                try fileManager.createDirectory(at: userDataFolder!, withIntermediateDirectories: true)
                try "[SimpleText]".write(to: configFilePath!, atomically: true, encoding: .utf8)
                print("Created config at: \(String(describing: configFilePath?.path))")
            }
        }catch{
            print("\(error)")
        }
        
    }
    
    func createWindowEditor() -> UUID{
        let newWindowEditor: WindowEditorController = WindowEditorController()
        let newWindowId: UUID = newWindowEditor.getWindowId()
        
        openWindows[newWindowId.uuidString] = newWindowEditor
        newWindowEditor.showWindow(nil)
        return newWindowId
    }
    
    func removeWindowEditor(windowId: UUID){
        openWindows.removeValue(forKey: windowId.uuidString)
        if(openWindows.count < 1 && quitRequested == true){
            NSApplication.shared.terminate(nil)
        }
    }
    
    func closeAllWindows(){
        for wE in openWindows.values{
            wE.window?.performClose(nil)
        }
    }
    
    /* Setting Apply to All Windows */
    func setEditorWordWrap(enabled: Bool){
        wordWrapEnabled = enabled
        appConfig?.write(keyToStore: "wordWrapEnabled", valueToStore: wordWrapEnabled ? "true" : "false")
        for wE in openWindows.values{
            wE.setWordWrap(enabled: enabled)
        }
    }
    func setEditorFont(newFont: NSFont){
        editorFont = newFont
        
        let fontName: String = editorFont!.fontName
        let fontSize: CGFloat = editorFont!.pointSize
        let fontDescription: String = "\(fontName), \(fontSize)"
        appConfig?.write(keyToStore: "editorFont", valueToStore: fontDescription)
        
        for wE in openWindows.values{
            wE.setTextFont(newFont: newFont)
        }
    }
    
    
    func showFontPanel(currentFont: NSFont?){
        NSFontPanel.shared.setPanelFont(currentFont ?? editorFont!, isMultiple: false)
        NSFontPanel.shared.makeKeyAndOrderFront(self)
    }
    
    /* App Menu Events */
    
    @IBAction func newMenuItemClicked(_ sender: NSMenuItem) {
        createWindowEditor()
    }
}
