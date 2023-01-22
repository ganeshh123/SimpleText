//
//  AppDelegate.swift
//  SimpleText
//
//  Created by Ganesh on 1/19/23.
//

import Cocoa

@main
class AppDelegate: NSObject, NSApplicationDelegate {
    
    var openWindows = [String : WindowEditorController]()
    // Application level settings
    var darkModeEnabled: Bool = false
    var wordWrapEnabled: Bool = true
    let defaultFont: NSFont? = NSFont(name: "Courier New", size: 18)
    
    
    func applicationDidFinishLaunching(_ aNotification: Notification) {
        // Remove annoying forced help menu customization
        let unusedMenu: NSMenu = NSMenu(title: "Help")
        NSApplication.shared.helpMenu = unusedMenu
        
        if #available(macOS 10.14, *){
            darkModeEnabled = UserDefaults.standard.string(forKey: "AppleInterfaceStyle") == "Dark"
        }
        
        createWindowEditor()
    }

    func applicationWillTerminate(_ aNotification: Notification) {
        // Insert code here to tear down your application
    }

    func applicationSupportsSecureRestorableState(_ app: NSApplication) -> Bool {
        return true
    }
    
    func createWindowEditor(){
        let newWindowEditor: WindowEditorController = WindowEditorController()
        let newWindowId: UUID = newWindowEditor.getWindowId()
        
        openWindows[newWindowId.uuidString] = newWindowEditor
        newWindowEditor.showWindow(nil)
    }
    
    func removeWindowEditor(windowId: UUID){
        openWindows.removeValue(forKey: windowId.uuidString)
    }
    
    /* Setting Apply to All Windows */
    func setEditorWordWrap(enabled: Bool){
        wordWrapEnabled = enabled
        for wE in openWindows.values{
            wE.setWordWrap(enabled: enabled)
        }
    }
    
    func showFontPanel(currentFont: NSFont?){
        NSFontPanel.shared.setPanelFont(currentFont ?? defaultFont!, isMultiple: false)
        NSFontPanel.shared.makeKeyAndOrderFront(self)
    }
    
    /* App Menu Events */
    
    @IBAction func newMenuItemClicked(_ sender: NSMenuItem) {
        createWindowEditor()
    }
}

