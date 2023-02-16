//
//  LoadFile.swift
//  SimpleText
//
//  Created by Ganesh on 2/16/23.
//

import Cocoa


extension AppDelegate{
    
    func application(_ sender: NSApplication, openFile filename: String) -> Bool {
        
        let targetWindowId: UUID = createWindowEditor()
        guard let targetWindow: WindowEditorController = openWindows[targetWindowId.uuidString] else{
            return false
        }
        
        targetWindow.readFile(file: URL(fileURLWithPath: filename))
        
        return true
    }
}
