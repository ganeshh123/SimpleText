//
//  AppDelegate.swift
//  SimpleText
//
//  Created by Ganesh on 1/19/23.
//

import Cocoa

@main
class AppDelegate: NSObject, NSApplicationDelegate {

    let windowEditor = WindowEditorController()

    func applicationDidFinishLaunching(_ aNotification: Notification) {
        // Insert code here to initialize your application
        windowEditor.showWindow(nil)
    }

    func applicationWillTerminate(_ aNotification: Notification) {
        // Insert code here to tear down your application
    }

    func applicationSupportsSecureRestorableState(_ app: NSApplication) -> Bool {
        return true
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
        
        windowEditor.readFile(file: result!)
    }
    


}

