   M S B A M L   `   `   `  ���� K  GEconomicTracking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:EconomicTrackingEconomicTracking  (    "EconomicTracking.SettlementReports   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:EconomicTracking     $��924.811��$��539.098��.��� .+��� $��Search��$
��Left=�$��	10,28,0,0q�$	��Top=�$��73��$	��905��.���   � ColumnDefinitions  �� $	��27* ��� $
��866* �.+��� $��Customer��$
��Left=�$	��Top=�$��0,10,0,0q�$	��153��$��40��$��2���� $��Select Settlement��$
��Left=�$	��Top=�$	��122��$��1��$��255.8,7,0,0q��� -    ��Name$ cbmSettlement��$
��Left=�$	��Top=�$	��211��$��1��$��392.8,10,0,0q��� -    ��Name$ Showdatabysetmt��$��	Show Data��$��Right=�$	��Top=�$	��128��$��0,12,91.8,0q�$��1��$y�Hidden=��� -    ��Name$ grpSettlement��$��Settlement Details��$
��Left=�$��
10,106,0,0q�$	��Top=�$	��393��$	��905��$y�Hidden=�.��� .+��� -   $ Showreportbutton��$��Show Report��$
��Left=�$	��Top=�$	��140��$��347,339,0,0q��� -   $ grpSettlementInfo��$��Settlement Information��$��10,7,10,264q�.��&   System.Windows.Controls.DataGrid  -     Name$ gridSettlement��$
��Left=�$	��Top=�   EnableRowVirtualization .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * �� -   $ gridSettlementcommodity��$��Commodity Information��$
��Left=�$	��283��$��315,112,0,55q�.��  -   $ gridCommodity��$
��Left=�$	��Top=�$	��172��$	��273��$ * $��	0,10,-2,0q��� -	   $ gridSettlementcurrency��$��Currency Information��$
��Left=�$	��281��$��10,112,0,55q�.��  -
   $ gridCurrency��$
��Left=�$	��Top=�$	��172��$	��269��$ * $��	2,10,-2,0q��� -   $ gridSettlementscrap��$��Scrap Information��$
��Left=�$	��274��$��619,112,0,55q�.��  -   $ 	gridScrap��$
��Left=�$	��Top=�$	��173��$	��264��$ * $��
0,10,-2,-1q�� $��0,28,-0.2,-27.8q�.+�� $��
0,-10,0,10q�.+�(  "System.Windows.Controls.DatePicker  -   $��dtSettleFrom��$
��Left=�$��500,100,0,0q�$	��Top=�  -   $��
dtSettleTo��$
��Left=�$��650,101,0,0q�$	��Top=� J� RenderTransformOrigin$ 1.125,0.908&��� $��Report Start Date��$
��Left=�$	��Top=�$	��102��$��
500,70,0,0q��� $��Report End Date��$
��Left=�$	��Top=�$	��102��$��
650,70,0,0q��� -   $ cbmCustomer��$
��Left=�$	��Top=�$	��211��$��	80,36,0,0q��� -   $��Show By date��$
��Left=�$��800,100,0,0q�$	��Top=�$��80��