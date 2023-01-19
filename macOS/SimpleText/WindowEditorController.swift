//
//  WindowEditorController.swift
//  SimpleText
//
//  Created by Ganesh on 1/19/23.
//

import Cocoa

class WindowEditorController: NSWindowController {
    
    let defaultFont: NSFont? = NSFont(name: "Courier New", size: 18)
    var openedFile: URL? = nil
    var openedFileInitialText: String = ""
    var fileModified: Bool = false
    
    
    
    @IBOutlet var textViewEditor: NSTextView!
    
    
    convenience init() {
        self.init(windowNibName: "WindowEditorController")
    }

    override func windowDidLoad() {
        super.windowDidLoad()

        textViewEditor.font = defaultFont
    }
    
    public func readFile(file: URL){
        print(file)
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
    
}
