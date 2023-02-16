//
//  Extensions.swift
//  SimpleText
//
//  Created by Ganesh on 2/16/23.
//

import Cocoa

extension NSView: NSFontChanging{
    
    /*
     For some reason NSView hijacks the changeFont event
     dispatched by the font selection panel, preventing it from
     getting to AppDelegate. Therefore, we need to pick up the
     new font here and give it to AppDelegate to update the other
     windows.
     */
    public func changeFont(_ sender: NSFontManager?){
        guard let fontManager = sender else {
            return
        }
        let newFont = fontManager.convert(NSFont.systemFont(ofSize: 13.0))
        
        guard let appDelegate = NSApplication.shared.delegate as? AppDelegate else {
             return
        }
        appDelegate.setEditorFont(newFont: newFont)
    }
}
