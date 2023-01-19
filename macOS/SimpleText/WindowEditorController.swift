//
//  WindowEditorController.swift
//  SimpleText
//
//  Created by Ganesh on 1/19/23.
//

import Cocoa

class WindowEditorController: NSWindowController {
    
    @IBOutlet var textViewEditor: NSTextView!
    
    
    convenience init() {
        self.init(windowNibName: "WindowEditorController")
    }

    override func windowDidLoad() {
        super.windowDidLoad()

        textViewEditor.font = NSFont(name: "Courier New", size: 18)
    }
    
}
