﻿#pragma checksum "..\..\..\Pages\StatisticsDataPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1C5ADBB9FC51A0510855687287D8A4E4149EE4A219A50333ED46B880D0C28A2C"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using AdminPanelBeta.Pages;
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


namespace AdminPanelBeta.Pages {
    
    
    /// <summary>
    /// StatisticsDataPage
    /// </summary>
    public partial class StatisticsDataPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\Pages\StatisticsDataPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxCategories;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Pages\StatisticsDataPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SalesTodayTextBlock;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Pages\StatisticsDataPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SalesLastMonthTextBlock;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Pages\StatisticsDataPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SalesLastYearTextBlock;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Pages\StatisticsDataPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SalesTotalTextBlock;
        
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
            System.Uri resourceLocater = new System.Uri("/AdminPanelBeta;component/pages/statisticsdatapage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\StatisticsDataPage.xaml"
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
            this.ComboBoxCategories = ((System.Windows.Controls.ComboBox)(target));
            
            #line 30 "..\..\..\Pages\StatisticsDataPage.xaml"
            this.ComboBoxCategories.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBoxCategories_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SalesTodayTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.SalesLastMonthTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.SalesLastYearTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.SalesTotalTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

