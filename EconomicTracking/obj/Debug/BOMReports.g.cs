﻿#pragma checksum "..\..\BOMReports.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "498BA00B12E94B2E5548DE128D39C769"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using RootLibrary.WPF.Localization;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace EconomicTracking {
    
    
    /// <summary>
    /// BOMReports
    /// </summary>
    public partial class BOMReports : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\BOMReports.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbmBom;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\BOMReports.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbmbomsetref;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\BOMReports.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bomreportbtn;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\BOMReports.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cuscombo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/EconomicTracking;component/bomreports.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\BOMReports.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cbmBom = ((System.Windows.Controls.ComboBox)(target));
            
            #line 8 "..\..\BOMReports.xaml"
            this.cbmBom.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbmBom_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cbmbomsetref = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.bomreportbtn = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\BOMReports.xaml"
            this.bomreportbtn.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.cuscombo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 14 "..\..\BOMReports.xaml"
            this.cuscombo.Loaded += new System.Windows.RoutedEventHandler(this.cuscombo_Loaded);
            
            #line default
            #line hidden
            
            #line 14 "..\..\BOMReports.xaml"
            this.cuscombo.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cuscombo_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

