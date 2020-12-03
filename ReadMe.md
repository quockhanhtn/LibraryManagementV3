# LIBRARY MANAGEMENT

<br>
<br>

## Overview                
- Programming language : **C#**
- UI framework : **Windows Presentation Foundation (WPF)**
- Design language : **Material Design** (Use Material Design In XAML package by [James Willock][ButchersBoy] and [Kevin Bost][Keboo])
- Relational database management system : **SQL Server**
- Object-relational mapping : **Entity Framework**
- Design pattern : **Model - View - ViewModel**
- Integrated development environment (IDE) : **Visual Studio 2019**

<br>
<br>

## Project Structure
<pre>
<b>LibraryManagement</b> (solution)
├── <b>LibraryManagement.App</b> (WPF App project)
│   ├── Model
│   │   ├── DAL (Data access layer)
│   │   │   ├── ... class insert, update, delete using EF DB Context
│   │   ├── DTO (Data transfer object)
│   │   │   ├── ... contain derived classes from EF DB Context
│   │   ├── Entity (*.edmx)
│   ├── ViewModel
│   ├── ... all View item will display here
├── <b>LibraryManagement.CustomControl</b> (WPF Custom Control Library project)
│   ├── ... 
├── <b>LibraryManagement.Utils</b> (Class library project)
└── LibraryManagement.sln
</pre>

<br>
<br>

## Project Team Member

| Name                  | Role		|
| ----------------------|-----------|
| **Hoang** Ho Huy      | Dev		|
| **Khanh** Lam Quoc    | PM		|
| **Nam** Tran Quoc     | Test		|
| **Phuc** Nguyen Tran  | BA		|

<br>
<br>

## Detail Plan
[File Excel](./DetailPlan.xlsx)

[ButchersBoy]: https://github.com/ButchersBoy
[Keboo]: https://github.com/Keboo
