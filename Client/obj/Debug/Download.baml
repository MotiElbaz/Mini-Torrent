   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5          Client.Page2   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      &localclr-namespace:Client     5        N� Title$  Download Manager��5      $	��565��6    $��910.5��6-   .��5
      � .+�5   
   �� -   5         ��Name$ Search_Label��5      $��Search :��6&   $
��Left=�69   $��	10,23,0,0q�6T   $	��Top=�6g   �� 5   
   -   5         ��Name$ Search_TextBox��5      $
��Left=�6*   $��25��6E   $��	61,26,0,0q�6Q    �� TextWrapping$
 Wrap=�6d   $	��Top=�6x   $	��190��6�   �� 5   
   -   5         ��Name$ 
Search_Btn��5      $��Search File��6%   $
��Left=�6;   $��
260,27,0,0q�6V   $	��Top=�6j   $��75��6�   6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ Search_DataGrid��5      $
��Left=�6,   $��	10,54,0,0q�6G   $	��Top=�6Z   $	��175��6r   $	��405��6      SelectionMode$ Single=�6�      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 	User name��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 68   �� *name+6S     5      $ IP��6%   	 61   �� *ip+6J   5        5   
   -   5        $ Files_DataGrid��5      $
��Left=�6+   $��
472,54,0,0q�6F   $	��Top=�6Z   $	��175��6r   $	��406��6   $ Single=�6�    5        5      $ 	File name��6%   	 68   �� *name+6S     5      $
 Size��6%   	 63   �� *size+6N   5      �� 5   
   -   5        $ Download_Btn��5      $��Download��6'   $
��Left=�6:   $��
14,246,0,0q�6U   $	��Top=�6i   $��75��6�   6�   � 5   
   -   5        
 �Name$
 ProgressBar��5      $
��Left=�6+   $��26��6F   $��107,246,0,0q�6R   $	��Top=�6g   $	��306��6   �� 5   
   -   5        $ Reflection_Label��5      $��Reflection : ��6*   $
��Left=�6B   $��468,246,0,0q�6]   $	��Top=�6r   �� 5   
   -	   5        $ Reflection_Btn��5      $
��Open��6)   $
��Left=�68   $��803,247,0,0q�6S   $	��Top=�6h   $��75��6�   $��25��6�   6�   �� 5   
   -
   5        $ 
Size_Lable��5      $��Size :��6$   $
��Left=�65   $��
173,54,0,0q�6P   $	��Top=�6d   �� 5   
   -   5        $ FileSize_Lable��5      $
��0 Kb��6(   $
��Left=�67   $��
227,54,0,0q�6R   $	��Top=�6f   �� 5    
   -   5        $ 
Time_Lable��5       $��Speed : 0 Kbps��6$   $
��Left=�6=   $��107,277,0,0q�6X   $	��Top=�6m    J� RenderTransformOrigin$ -1.415,0.577&�6�   �� 5!   
   -   5        $ Speed_Lable��5!      $��Time : ��6%   $
��Left=�67   $��260,277,0,0q�6R   $	��Top=�6g   �� 5"   
   -   5        $ Search_textBox��5"      $
��Left=�6*   $��23��6E   $��540,249,0,0q�6Q   $
 Wrap=�6f   $�� ��6z   $	��Top=�6�   $	��258��6�   5#      5$      