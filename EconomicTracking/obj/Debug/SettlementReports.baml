   M S B A M L   `   `   `  ����K  GEconomicTracking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:EconomicTrackingEconomicTracking  5      (    "EconomicTracking.SettlementReports   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:EconomicTracking     5      $��924.811��5      $��539.098��6/   .��5      � .+�5	   
   �� $��Search��6   $
��Left=�6#   $��	10,28,0,0q�6>   $	��Top=�6Q   $��73��6i   $	��905��6u   .��5
      �   � ColumnDefinitions  5      �� 5      $	��27* �6'   �� 5      $
��866* �6'   .+�5      �� $��Customer��6   $
��Left=�6+   $	��Top=�6F   $��0,10,0,0q�6^   $	��153��6p   $��40��6|   $��2��6�   �� 5      $��Select Settlement��6   $
��Left=�64   $	��Top=�6O   $	��122��6g   $��1��6s   $��255.8,7,0,0q�6�   �� 5      -   5         ��Name$ cbmSettlement��5      $
��Left=�62   $	��Top=�6N   $	��211��6f   $��1��6r   $��392.8,10,0,0q�6�   6�   �� 5      -   5         ��Name$ Showdatabysetmt��5      $��	Show Data��62   $��Right=�6F   $	��Top=�6b   $	��128��6z   $��0,12,91.8,0q�6�   $��1��6�   $y�Hidden=�6�   5      5      �� 5   
   -   5         ��Name$ grpSettlement��5      $��Settlement Details��6*   $
��Left=�6F   $��
10,106,0,0q�6a   $	��Top=�6u   $	��393��6�   $	��905��6�   $y�Hidden=�6�   .��5      � .+�5      �� -   5        $ Showreportbutton��5      $��Show Report��63   $
��Left=�6I   $	��Top=�6d   $	��140��6|   $��347,339,0,0q�6�   �� 5      -   5        $ grpSettlementInfo��5      $��Settlement Information��66   $��10,7,10,264q�6V   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ gridSettlement��5         AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly . $
��Left=�6|   $	��Top=�6�      EnableRowVirtualization .  	   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$	 * 6�   
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 
CustomerId��6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *
CustomerId+6P     5      $ CustomerName��6R    61   �� *CustomerName+6R     5      $ SettlementFromDate��6s    61   �� *SettlementFrom+ �� StringFormat$ {0:dd MMM yyyy}��6s     5      $ SettlementToDate��6q    61   �� *SettlementTo+$ {0:dd MMM yyyy}��6q     5      $ SettlementRef��6S    61   �� *SettlementRef+6S   5!      5"      �� 5#      -   5        $ gridSettlementcommodity��5#      $��Commodity Information��6<   $
��Left=�6[   $	��283��6v   $��315,112,0,55q�6�   .��5$        -   5        $ gridCommodity��5$      $
��Left=�66   $	��Top=�6Q   $	��172��6i   $	��273��6v   $	 * 6�   $��	0,10,-2,0q�6�   5%      �� 5&      -	   5        $ gridSettlementcurrency��5&      $��Currency Information��6;   $
��Left=�6Y   $	��281��6t   $��10,112,0,55q�6�   .��5'        -
   5        $ gridCurrency��5'      $
��Left=�65   $	��Top=�6P   $	��172��6h   $	��269��6u   $	 * 6�   $��	2,10,-2,0q�6�   5(      �� 5)      -   5        $ gridSettlementscrap��5)      $��Scrap Information��68   $
��Left=�6S   $	��274��6n   $��619,112,0,55q�6z   .��5*        -   5        $ 	gridScrap��5*      $
��Left=�62   $	��Top=�6M   $	��173��6e   $	��264��6r   $	 * 6~   $��
0,10,-2,-1q�6�   5+      5,      5-      � 5/   
   $��0,28,-0.2,-27.8q�6   .+�50      � $��
0,-10,0,10q�6   .+�51      (  "System.Windows.Controls.DatePicker  -   5        $��dtSettleFrom��51      $
��Left=�6      SelectedDateFormat$
 Long=�68   $��500,100,0,0q�6R   $	��Top=�6g   53        54      -   5        $��
dtSettleTo��54      $
��Left=�6   $��650,101,0,0q�68   $
 Long=�6M   $	��Top=�6g    J� RenderTransformOrigin$ 1.125,0.908&�6�   �� 55      $��Report Start Date��6   $
��Left=�64   $	��Top=�6O   $	��102��6g   $��
500,70,0,0q�6s   �� 56      $��Report End Date��6   $
��Left=�62   $	��Top=�6M   $	��102��6e   $��
650,70,0,0q�6q   �� 57      -   5        $ cbmCustomer��57      $
��Left=�60   $	��Top=�6L   $	��211��6d   $��	80,36,0,0q�6p   59      �� 5:      -   5        $��Show By date��5:      $
��Left=�60   $��800,100,0,0q�6K   $	��Top=�6`   $��80��6x   6�   5<      5>      5B      5F      