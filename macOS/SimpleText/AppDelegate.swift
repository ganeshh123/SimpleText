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

    func applicationDidFinishLaunching(_ aNotification: Notification) {
        // Remove annoying forced help menu customization
        let unusedMenu: NSMenu = NSMenu(title: "Help")
        NSApplication.shared.helpMenu = unusedMenu
        
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
    
    /* App Menu Events */
    
    @IBAction func newMenuItemClicked(_ sender: NSMenuItem) {
        createWindowEditor()
    }
}

