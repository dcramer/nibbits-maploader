﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30128.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nibbler.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Client : global::System.Configuration.ApplicationSettingsBase {
        
        private static Client defaultInstance = ((Client)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Client())));
        
        public static Client Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SC1_INSTALL_PATH {
            get {
                return ((string)(this["SC1_INSTALL_PATH"]));
            }
            set {
                this["SC1_INSTALL_PATH"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SC2_INSTALL_PATH {
            get {
                return ((string)(this["SC2_INSTALL_PATH"]));
            }
            set {
                this["SC2_INSTALL_PATH"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string WC3_INSTALL_PATH {
            get {
                return ((string)(this["WC3_INSTALL_PATH"]));
            }
            set {
                this["WC3_INSTALL_PATH"] = value;
            }
        }
    }
}