   M S B A M L   `   `   `  ����F  BArchivesCar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ArchivesCar.ViewArchivesCar.View  5      "    ArchivesCar.View.HomeControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:ArchivesCar.View     5      .��5      (�   (� Stretch$
  Fill=�6   .��5	   
   � $
��1920��6   $
��1024��6   ��5
   
   �� 5       �� ImageSource$" ../image/盘点车背景.png��6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��1.8* �6   �� 5      $
��1.8* �6   �� 5      $��* �6   .+�5   
   �� $&�� ../image/Homeimage/盘点车.png��6   $��50,0,0,0q�64   $
��Left=�6F   $��Center=�6a   $	��345��6|   $��77��6�   �� 5   
   -   5           EmptyButtonStyle#��[  5      $��Right=�6;   $��50��6W   $	��105��6b   $��0,0,30,0q�6o   ��5      �� 5      $#��../image/HomeImage/退出.png��6   5      � 5   
    � Orientation$ 
Horizontal=�6   $��1��6-   $��Center=�6:   $��Center=�6U   .+�5      �� #��[  6   $	��300��6?   ��6L   �� *InventoryCommond+��5      �� 5      $#��../image/HomeImage/盘点.png��6   5      �� 5      $	��100��6   �� 5       #��[  6   $	��300��6?   ��6L   �� *QueryCommond+��5!      �� 5"      $#��../image/HomeImage/查询.png��6   5$      �� 5%      $	��100��6   �� 5&      #��[  6   $	��300��6?   ��6L   �� *BindCommond+��5'      �� 5(      $)��#../image/HomeImage/档案绑定.png��6   5*      5+      � 5,   
   $ 
Horizontal=�6   $��2��6-   $��Center=�6:   $��Center=�6U   .+�5.      �� #��[  6   $	��300��6?   ��6L   �� *UpperShelfCommond+��5/      �� 50      $#��../image/HomeImage/上架.png��6   52      �� 53      $	��100��6   �� 54      #��[  6   $	��300��6?   ��6L   �� *ChangeCommond+��55      �� 56      ��6   �� *PIC+58      59      � 5:   
   $��3��6   $ 
Horizontal=�6!   $��Center=�6:   $��Center=�6U   .+�5;      �� $#��../image/HomeImage/时间.png��6   $��30��67   �� 5<      $��26(�6   
�������$��10,0,0,0q�6N   ��6"   �� *Time+6N   5=      �� 5>   
   $��3��60   
�������$��Center=�6P   $��26(�6t   ��6   �� *ChangeModel+6t   5?      5@      5A      