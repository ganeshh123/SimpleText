//
//  IniFile.swift
//  SimpleText
//
//  Created by Ganesh on 2/15/23.
//

import Foundation

class IniFile {
    
    private final let path: String
    
    init(iniPath: String){
        path = iniPath
    }
    
    public func read(queryKey: String) -> String?{
        guard let configFile = freopen(path, "r", stdin) else { return nil}
        defer{
            fclose(configFile)
        }
        while let line = readLine(){
            let lineComponents: [String] = line.components(separatedBy: "=")
            if(lineComponents.count != 2){
                continue
            }
            let key: String = lineComponents[0]
            let value: String = lineComponents[1]
            
            if(key == queryKey){
                return value
            }
        }
        return nil
    }
    
    public func write(keyToStore: String, valueToStore: String) -> Void{
        guard let configFile = freopen(path, "r", stdin) else { return }
        defer{
            fclose(configFile)
        }
        
        var configFileLines: [String] = []
        var targetLine: Int = -1;
        let lineToWrite: String = "\(keyToStore)=\(valueToStore)";

        while let line = readLine(){
            configFileLines.append(line)
            
            let lineComponents: [String] = line.components(separatedBy: "=")
            if(lineComponents.count != 2){
                continue
            }
            let key: String = lineComponents[0]
            
            if(key == keyToStore){
                targetLine = configFileLines.count
            }
        }
        
        if(targetLine != -1){
            configFileLines[targetLine - 1] = lineToWrite
        }else{
            configFileLines.append(lineToWrite)
        }
        
        let configFileToWrite = configFileLines.joined(separator: "\n")
        
        // Close file opened with C function
        fclose(configFile)
        do{
            try configFileToWrite.write(toFile: path, atomically: true, encoding: .utf8)
        }
        catch{
            print("Error occured writing to config file \(error)")
        }
    }
}
