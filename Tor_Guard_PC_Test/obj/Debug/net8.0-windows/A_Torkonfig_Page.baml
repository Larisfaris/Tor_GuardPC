   M S B A M L   `   `   `  ����  Tor_Guard_PC_Test    TorGuard.Torkonfig_Page   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  Tor Konfiguration��5      $
��Auto��6!   $
��Auto��6/   .��5      � $��10q�6   
������� � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   $	��300��6,   �� 5      $	��200 �6   �� 5      $	��200 �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6!   $��Tor Konfiguration��61   $��18(�6J   $
��Bold"�6X   $��5,5,5,5q�6j   �� 5   
   -   5         ��Name$ listBoxTore��5   T   $��1��6   $��0��6   $��2��6/   $��1��6@   $��5,5,5,5q�6�   $	��400��6�   $	��500��6�   �� 5    
   $��2��6   $��1��6"    �� Orientation$ Vertical=�62   $��5,5,5,5q�6I   $��2��6Z    � 	Resources 5!       �� 
TargetType �� Property �� Value��%4  '��       '�� k     �� 5"      " ��6   .�5#      ��  � ��$ 12(�61   �� 5$       � ��$ 5q�6/   5%      �� 5&      " ��6   .�5'      ��  � ��$	 200��61   �� 5(       � ��$	 400��61   �� 5)       � ��$ 12(�61   �� 5*       � ��$ 5q�6/   5+      5,      .+�5.      �� $��	Tor Name:��6   �� 5/      -   5        	 ��Name$	 
txtTorName��5/      �� 51      $��GPIO Pin Indoor:��6   �� 52      -   5        $	 txtGpioPinIndoor��52      �� 54      $��GPIO Pin Outdoor:��6   �� 55      -   5        $	 txtGpioPinOutdoor��55      �� 57      $��Kamera Indoor URL:��6   �� 58      -   5        $	 txtKameraIndoorUrl��58      �� 5:      $��Kamera Outdoor URL:��6   �� 5;      -   5        $	 txtKameraOutdoorUrl��5;      �� 5=      $ ��Speicherort Kamera Indoor:��6   �� 5>      -   5        $ 	 txtSpeicherortKameraIndoor��5>      �� 5@      $!��Speicherort Kamera Outdoor:��6   �� 5A      -   5        $!	 txtSpeicherortKameraOutdoor��5A      �� 5C      $��Speicherort Crash:��6   �� 5D      -	   5        $	 txtSpeicherortCrash��5D      5E      �� 5H   
   $��3��6   $��1��6"   $ 
Horizontal=�62   $��Center=�6K   $��0,5,0,5q�6h   $	��480��6y   $��11��6�    5I      ��%v   '��       �� 5J      " ��6   .�5K      ��  � ��$	 150��6.   �� 5L       � ��$ 5q�6/   5M      5N      .+�5P      �� -
   5        $��	Speichern��5P      6)   �� 5Q      -   5        $��Neues Tor hinzufügen��5Q      64   �� 5R      -   5        $��Konfig Verlassen��5R      60   5S      5U      5V      