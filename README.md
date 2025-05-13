# <p align="center">**Console Vocabulary**</p>
## **0. Basics**
### *0.1. Menu item colors (Fig. 1)*
- *white / light blue* – unselected / selected collection item;;
- *yellow* - execute operation;
- *green* - open submenu;
- *purple* - execute operation as administrator;
- *red* - dangerous operation (e.g., deletion);
- *gray* - go back / cancel operation.
![image](https://github.com/user-attachments/assets/127aa2d4-9068-4a7e-88e4-16d902caa992)
<p align="center"><i>Fig. 1. Menu item colors</i></p>

### *0.2. Saving to a text file*
When entering the save menu, the program will prompt the user to select a file from the desktop, but the user can change the file location at their discretion (Fig. 2).
![image](https://github.com/user-attachments/assets/76506b0a-da90-4803-bf1e-88313914ad34)
<p align="center"><i>Fig. 2. Saving the dictionary to a text file</i></p>

The text file contains a tabular representation of the words. It is recommended to use a monospaced font for viewing, so it's best to open the file in a code editor (e.g., Visual Studio Code (Fig. 3)) or change the font in a program that allows it (e.g., Notepad++). Otherwise, the displayed text may look incorrect (Fig. 4).
![image](https://github.com/user-attachments/assets/db4c1e3e-c352-4642-8e49-c9f2e4eb6dcc)
<p align="center"><i>Fig. 3. Dictionary text file in Visual Studio Code</i></p>

![image](https://github.com/user-attachments/assets/5f73237a-9601-4f3d-85ea-1a6eef87526c)
<p align="center"><i>Fig. 4. Dictionary text file in Notepad</i></p>

## 1. User Interface (Fig. 5)
![image](https://github.com/user-attachments/assets/7958336c-ea3f-4dab-800a-444c9d115fd0)
<p align="center"><i>Fig. 5. Main user menu</i></p>

### *0.1. Dictionary view*
The dictionary is presented to the user as a table (Fig. 6).
![image](https://github.com/user-attachments/assets/174133a0-6cfa-4bdb-885b-777490dc4d00)
<p align="center"><i>Fig. 6. English–French dictionary (user interface)</i></p>

A search bar allows searching by spelling and/or transcription of the original word and/or its translation. Just start typing (Figs. 7–9).

![image](https://github.com/user-attachments/assets/79d04217-511c-48fe-9a73-b0d757d76b8a)
<p align="center"><i>Fig. 7. Search by original word spelling</i></p>

![image](https://github.com/user-attachments/assets/b27258cc-6298-4b48-91cd-3ed1d0a4b6fe)
<p align="center"><i>Fig. 8. Search by original word transcription</i></p>

![image](https://github.com/user-attachments/assets/6510ba25-1df9-4ccc-8755-81138f20a662)
<p align="center"><i>Fig. 9. Search by translation spelling</i></p>

### *0.2. Usage / similar words*
The program considers usage of a word to be words that completely contain the selected word (case-insensitive) (Fig. 10). Words similar to the selected one are those that contain at least one word from the selected phrase (also case-insensitive) (Fig. 11).\
*P.S. By “word” a word or phrase entered in a single dictionary “cell.” is meant*
![image](https://github.com/user-attachments/assets/c8648f26-ed34-4d03-9e18-900ffff0f4e7)
<p align="center"><i>Fig. 10. Usage of the word “пожалуйста” (rus. “please”)</i></p>

![image](https://github.com/user-attachments/assets/47ce071c-e7be-4322-b65f-bc33d7305b68)
<p align="center"><i>Fig. 11. Words similar to “Вы давно тут?” (“Have you been here long?”)</i></p>

## **2. Admin Interface (Fig. 12)**
![image](https://github.com/user-attachments/assets/fd91519a-5f89-44b7-9026-d5a03d7644b2)
<p align="center"><i>Fig. 12. Main admin menu</i></p>

The admin interface differs in two aspects: the ability to create/delete dictionaries and modify their contents, and a simplified dictionary view (Fig. 13).
![image](https://github.com/user-attachments/assets/9a7afa56-21cf-45eb-af64-535897f0ee3a)
<p align="center"><i>Fig. 13. English–French dictionary (admin interface)</i></p>

***To log in as an administrator, enter the password on the start screen and press Enter.*** If the password is incorrect, the user menu will open.\
***The default password is “qwerty”***; after the first login, the admin can change it.
## **3. Possible compatibility issues**
Not all consoles support text formatting, yet one of the key features of the program is “striking through” unavailable operations. If you cannot select an operation and are unsure why, check the following:
- the user cannot open a dictionary that does not exist (Fig. 14);
- the user cannot view the usage/similar words list if it is empty (Fig. 15);
- the admin cannot delete a word’s translation if it is the last one (Fig. 16).
![image](https://github.com/user-attachments/assets/4f82e7f4-bf5f-4732-97db-88e0f465b7e3)
<p align="center"><i>Fig. 14. Struck-through non-existent dictionaries</i></p>

![image](https://github.com/user-attachments/assets/fc577b25-4042-4dee-aa88-ac0da5bcef50)
<p align="center"><i>Fig. 15. Struck-through empty usage list</i></p>

![image](https://github.com/user-attachments/assets/b7dba9c6-f59b-4077-abec-8058e66b96de)
<p align="center"><i>Fig. 16. Struck-through option to delete the last translation</i></p>
