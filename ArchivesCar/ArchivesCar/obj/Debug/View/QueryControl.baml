   M S B A M L   `   `   `  ����F  BArchivesCar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ArchivesCar.ViewArchivesCar.View  5      #    ArchivesCar.View.QueryControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:ArchivesCar.View     5      .��5      (�   (� Stretch$
  Fill=�6   .��5	   
   � $
��1920��6   $
��1024��6   ��5
      �� 5       �� ImageSource$ ../image/set.jpg��6    � RowDefinitions 5      �� 5      $��0.14* �6    �� 5      $
��0.9* �6    .+�5      �  � Orientation$ 
Horizontal=�6   $��Center=�61   .+�5      ��    EmptyButtonStyle#��[  6   $
��Left=�6C   $��45��6^   $��45��6i   $��50,0,0,0q�6u   ��6�   �� *BackCommond+��5      �� 5      $��../image/back.png��6    5      �� 5      #��[  6   $��Center=�6C   $��	-50,0,0,0q�6`   ��6s   �� *BackCommond+��5      �� 5      $��../image/back1.png��6    $��25��6<   5      �� 5      #��[  6   ��6C   �� *BackCommond+��5      �� 5      $��返回��6    $��30(�6-   $��Center=�6;   
�������6]   5       5!      � 5"      $ Vertical=�6   $��Center=�6/   $��Center=�6L   .+�5#      �� #��[  6   $	��432��6C   $��46��6O   ��6[   �� *ChangeCommond+��5$      �� 5%      ��6    �� *PIC+5'      �� 5(      $��Center=�62   $��Center=�6T   $��26(�6x   
���������6   �� *Total+6�   �� 5)      $��Center=�62   $��Center=�6T   $��26(�6x   
���  ����6   �� *Error+6�   5+      �� 5,      $	��200��6   $��Right=�6    $��Right=�6V   $��Center=�6y   $��30(�6�   
�������$��0,0,50,0q�6�   ��6<   �� *Model+6�   &   System.Windows.Controls.DataGrid  5-      -   5        $
��grid��5-   4   $��1��6@   $��0q�6M   
�������   ColumnHeaderHeight$ 60Q�6v      AutoGenerateColumns .     CanUserAddRows .  ��6   �� *List+   ColumnHeaderStyle 5.      �� 5/       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�50      �� 	 �� Property	 � ��
 �� Value$
 Center=�6H   �� 51      	 � ��$
 31(�65   �� 52      	 � ��
 �#19B8FB67   �� 53      	 � ��
 �#D4FAFF67   54         RowStyle 57      �� 58      )  #System.Windows.Controls.DataGridRow"  6   .�59      �� 	 � ��$
 28(�65   �� 5:      	 � ��$
 0q�6<   �� 5;      	 � ��
 �#66666667   5<         	CellStyle 5>      �� 5?      "  6   .�5A      �� 	 � ��$
 0q�6<   5B         Columns 5D      4  .System.Windows.Controls.DataGridTemplateColumn  5E      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.3* 6-      Header$ 序号��6:      ClipboardContentBinding 6F   Z�    CellTemplate 5F      �� 5G      .R�5H   "   �� $��28(�6B   ���#666666$��Center=�6e   $��Center=�6�   ��6(   �� *order+6�   5I      5K        5L      $ * 6-   $ 题名��67    6C   Z�  5M      �� 5N      .R�5O   "   �� $��	678678674��6(   $��28(�6<   ���#666666$��Center=�6_   $��Center=�6�   5P      5R        5S      $ * 6-   $ 类别��67    6C   Z�  5T      �� 5U      .R�5V   "   �� $��28(�6B   ���#666666$��Center=�6e   $��Center=�6�   ��6(   �� *isBox+6�   5W      5Y        5Z      $ * 6-   $ 条码��67    6C   Z�  5[      �� 5\      .R�5]   "   �� $��28(�6D   ���#666666$��Center=�6g   $��Center=�6�   ��6(   �� *	barCode+6�   5^      5`        5a      $ * 6-   $
 RFID��67    6E   Z�  5b      �� 5c      .R�5d   "   �� $��28(�6A   ���#666666$��Center=�6d   $��Center=�6�   ��6(   �� *rfid+6�   5e      5g        5h      $ * 6-   $ 存放位置��67    6E   Z�  5i      �� 5j      .R�5k   "   �� $��28(�6I   ���#666666$��Center=�6l   $��Center=�6�   ��6(   �� *locationName+6�   5l      5n      5p      5q      5r      5s      