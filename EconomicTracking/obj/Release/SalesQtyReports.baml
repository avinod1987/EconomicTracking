   M S B A M L   `   `   `  ����   EconomicTracking&     EconomicTracking.SalesQtyReports   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .��]� #��� ��.+�� $	��Top=�$	��152��$	��778��  � RowDefinitions  �� $
��Auto ��� $
��Auto ��� $��* � � ColumnDefinitions �� $
��Auto ��� �� $
��Auto �.+��� -   $��Export Data...��$��32��$��690,16,-0.1,-0.4q�$
��Left=�$	��Top=�$��4,2,4,4q�#��� ��$��0q�$��1��$��2��$��3��$��82����Z� �� -    ��Name$ lblFileName��$��1��$��
360,3,0,14q�#��� ��$
��Left=�$��Sales Qty Date : ��$��86���� -   $ lblFileName_Copy��$��1��$��461,22,0,41.6q�#��� ��$
��Left=�$
�� To ��$��19�� J� RenderTransformOrigin$ 11.737,0.575&�$��3���� -   $��Search��$��1��$
��Left=�$��
590,20,0,0q�$	��Top=�$��75��#��� ��$��22��(  "System.Windows.Controls.DatePicker  -   $��	dtSalesTo��$��1��$
��Left=�$��
480,20,0,0q�$	��Top=�$��24��$	��102��  -   $��dtSalesFrom��$��1��$
��Left=�$��
360,20,0,0q�$	��Top=�$��24��$	��102���� -    ��Name$ cuscombo��$��2��$
��Left=�$	��Top=�$	��146��$��	21,20,0,0q�  -   $��dtsalesfrommonth��$��1��$
��Left=�$��	388,5,0,0q�$	��Top=�$��24��$	��102��$��2��  -	   $��dtsalesfromYear��$��1��$
��Left=�$��	504,5,0,0q�$	��Top=�$��24��$	��102��$��2���� -
    ��Name$ salesqtybymonthbtn��$��SalesQtyByMonth��#��� ��$��2��$
��Left=�$	��Top=�$	��128��$��	606,5,0,0q�$��2���� -    ��Name$ chkSelectAll��$y�Visible=�$��
Select All��$��1��$
��Left=�$��
180,92,0,0q�$��2��$	��Top=��� -    ��Name$ lstItems�� �� SelectionMode$ Multiple=�$��1��$��180,16,435.6,16q�$��3��$
��Auto=����� ���� .R�$� .+��� $ chkcus��-   ���� 	 �� Path$	 CustomerAssemblyId�@��� -   
 ��Name$
 salesqtygroup��$��Sales Quality Details��$��10,7,10,264q�$y�Hidden=�.��&   System.Windows.Controls.DataGrid  -     Name$ salesqtydatagrid��$
��Left=�$	��Top=�   EnableRowVirtualization .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 