   M S B A M L   `   `   `  ����  EconomicTracking&     EconomicTracking.SalesQtyReports   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5         myHeaderStyle �� 
TargetType �� Property �� Value��%Y   &	        �� 5   
   " ��6&   .�5	      ��  � y�$ 	Collapsed=�6+   5
      5      .��5      ]� #��� ��6   .+�5   
   � $	��Top=�6   $	��164��6$   $	��778��61    � RowDefinitions 5      �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $��* �6    6-    � ColumnDefinitions 5      �� 5      $
��Auto �6#   62   �� 5      �� 5      $
��Auto �6#   62   .+�5      �� -   5        $��Export Data...��5      $��32��6.   $��690,16,-0.1,-0.4q�6:   $
��Left=�6T   $	��Top=�6o   $��4,2,4,4q�6�   #��� ��6�   $��0q�6  $��1��6!  $��2��61  $��3��6E  $��82��6V  ��6�   Z� 6V  �� 5      -   5         ��Name$ lblFileName��5   (   $��1��6   $��
360,3,0,14q�6=   #��� ��6Q   $
��Left=�6�   $��Sales Qty Date : ��6�   $��86��6�   �� 5      -   5        $y�Hidden=�5      $��Search��6*   $��1��6;   $
��Left=�6K   $��
590,20,0,0q�6f   $	��Top=�6z   $��75��6�   #��� ��6�   $��22��6�   (  "System.Windows.Controls.DatePicker  5      -   5        $��	dtSalesTo��5   �   $y�Hidden=�6   $��1��6-   $
��Left=�6=   $��
480,20,0,0q�6X   $	��Top=�6l   $��24��6�   $	��102��6�     5      -   5        $��dtSalesFrom��5   �   $y�Hidden=�6   $��1��6-   $
��Left=�6=   $��
360,20,0,0q�6X   $	��Top=�6l   $��24��6�   $	��102��6�   �� 5      -   5         ��Name$ cuscombo��5      $��2��6)   $
��Left=�6=   $	��Top=�6X   $	��146��6p   $��10,0,0,0q�6|   $��2��6�     5      -   5        $��dtsalesfrommonth��5   �      SelectedDateFormat$
 Long=�6   $��1��63   $
��Left=�6C   $��351,3.2,0,0q�6^   $	��Top=�6s   $��24��6�   $	��143��6�   $��2��6�     5       :  4System.Windows.Controls.Primitives.DatePickerTextBox��%m   '        �� 5!      "  6   .�5"      ��  � ��$ Enter SalesQty From Date��61   6T   5#      5$      5%        5&      -   5        $��dtsalesfromYear��5&   �   $
 Long=�6   $��1��64   $
��Left=�6D   $��499,2.2,0,0q�6_   $	��Top=�6t   $��24��6�   $	��125��6�   $��2��6�     5'      ��%k   '        �� 5(      "  6   .�5)      ��  � ��$ Enter SalesQty To Date��61   6R   5*      5+      5,      �� 5.      -	   5        	 ��Name$	 salesqtybymonthbtn��5.   w   $��SalesQtyByMonth��6   #��� ��6/   $��2��6�   $
��Left=�6�   $	��Top=�6�   $	��128��6�   $��640,3.2,0,0q�6�   $��2��6�   6  �� 5/      -
   5        
 ��Name$
 chkSelectAll��5/      $y�Visible=�6-   $��
Select All��6B   $��1��6W   $
��Left=�6g   $��
180,92,0,0q�6�   $��2��6�   $	��Top=�6�   6�   �� 50      -   5        $	��trt��50   E   $��2��6   $��180,25,431.6,32q�6,   $��3��6P    �� View 51      �� 52      ! �� ColumnHeaderContainerStyle# [  6   .P�53      ��  �� CellTemplate 54      �� 55   "   .R�56   &   �� $	��tgt��6/   -   5         d� 	IsChecked 56   S   ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" �� �� Path$ 
IsSelected���6�   �� $ CustomerAssemblyId�6  57   #   59      5:      5=      �� 5>      $��2��6   $��CustomerName��6(   $
��Left=�6?   $��	10,20,0,0q�6Z   $��3��6m   $	��Top=�6~   $	��146��6�   �� 5?      $��CustomerAssemblyId��6    J� RenderTransformOrigin$ -2.646,-11.466&�61   $��1��6X   $��180,0,472.6,19.6q�6h   5@      � 5B   
   .+�5C      � .+�5D      �� -   5         ��Name$ salesqtygroup��5D      $��Sales Quality Details��62   $��10,7,10,264q�6Q   $y�Hidden=�6f   .��5E      &   System.Windows.Controls.DataGrid  -   5          Name$ salesqtydatagrid��5E         CanUserAddRows .     AutoGenerateColumns .     
IsReadOnly . $
��Left=�6~   $	��Top=�6�      EnableRowVirtualization .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6�      Columns 5F      0  *System.Windows.Controls.DataGridTextColumn  5G      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Month Year��6O   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *
montyear+6O     5H      $ SalesQuantity��6I    61   �� *Sum+6I     5I      $ CustomerAssemblyId��6Y    61   �� *CustomerAssemblyId+6Y   5K      5L      5N      �� 5O      -   5        $��gg��5O      
������� J� Opacity$	 0.7Q�6/   y�6=   �� *	IsWaiting+�� 5P      -   5        $	��txt��5P      $��Please Wait��6#   $��Center=�66   $��20(�6S   $
��Bold"�6a   $��Center=�6s   $��38��6�   $	��188��6�   6�   5R      5U      5V      