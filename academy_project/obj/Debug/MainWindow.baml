   M S B A M L   `   `   `  ����J  Facademy_project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:academy_projectacademy_project  5   	        academy_project.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:academy_project     5   	   -   5          � Title$  
MainWindow��5   	   $	��300��6   $	��550��6)    � 
ResizeMode$ NoResize=�65    � WindowStyle$
 None=�6K   
������  � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�6�   .��5	      �� ���#C4D2EC �� CornerRadius$ 20��6"   ._�5
   
   � $��10q�6   ���#C4D2EC � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6     � ColumnDefinitions 5      �� 5      $
��407* �6#   �� 5      $
��124* �6#   .+�5      A� -   5        $��0��5      $��24��6#   $��24��6/   $��Right=�6:   $	��Top=�6V   $��1��6�   $��
0,0,-0.6,0q�6�    � Cursor$
 Hand��6�   ��5      �� 5      	 �� ImageSource$	 x5.png��6!   5      �� 5      $%��Добро пожаловать��6   $
��Bold"�6/   $��Center=�6A   $��Center=�6^   $��30(�6y   $��0��6�   $��0��6�   $��119,20,118.4,20.4q�6�   $��2��6�   �� 5      $��1��6   
 �� Orientation$
 Vertical=�6&   $��Center=�6=   $��Center=�6Z   $��141,0.6,17.879,0.4q�6u   .+�5       �� -   5         ��Name$ 	login_btn��5       $
��Bold"�6,   $��
Войти��6>   $��34��6N   $	��208��6Z   ���#EFEFEF$��20q�6{   $��0q�6�    � 	Resources 5!       �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value��%�   '��       �� 5"      " ��6    .�5#      ��  �@��CornerRadius$ 20��6=    5$      P� 5%   "    �@J�IsMouseOver . . �5&   &   ��  � �� �#8A16166C   5'   #   5)      5*      5+      �� 5-      -   5        $ registration_btn��5-      $
��Bold"�63   $*��$Зарегистрироваться��6E   $��34��6b   $	��208��6n   ���#EFEFEF$
��0,10q�6�   $��0q�6�    5.      ��%_   '��       �� 5/      " ��6    .�50      ��  �@��CornerRadius$ 20��6=   51      52      53      54      55      56      57      