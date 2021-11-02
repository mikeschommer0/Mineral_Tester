# Coding Standards
## Team 5
## Rick Bowman, Quinn Nimmer, Mike Schommer, Seth Frevert

### **C#**
- 1 Naming Conventions
    - PascalCase
        - Classes
        - Methods
        - Enumerations
        - Public fields and properties
        - Namespaces
        - File names
    - camelCase
        - Local variables
        - Parameters
    - camelCase with _ prefix
        - Private, protected, internal, and protected internal fields and properties
- 2 Interface names being with I
    - IDatabase, IAnimal
- 3 Do not use prefixes in file names
    - Correct: User, UserTest
    - Incorrect: cUser (class), utUser (unit test)
- 4 When using a Switch statement, remove the default case if it will never be reached
- 5 Methods longer than 20 lines should be refactored if possible
- 6 Methods can be a maximum of 30 lines long
- 7 User American English spellings
    - Correct: color
    - Incorrect: colour
- 8 Braces belong on their own line
    - Correct:
        ```cs
            class MyClass
            {
                void DoSomething()
                {
                    if (condition)
                    {
                        //do something
                    }
                    else
                    {
                        //do something else
                    }
                }
            }
        ```
    - Incorrect:
        ```cs
            class MyClass {
                void DoSomething() {
                    if (condition) {
                        //do something
                    } else {
                        //do something else
                    }
                }
            }
        ```
- 9 Use braces even if they are optional
    - Correct:
        ```cs
            if (someCondition)
            {
                return true;
            }
        ```
    - Incorrect:
        ```cs
            if (someCondition)
                return true;
        ```
- 10 Write only one statement per line
    - Correct:
        ```cs
            int a = 1;
            int b = 2;
        ```
    - Incorrect:
        ```cs
            int a = 1; int b = 2;
        ```
 - 11 Declare only one field or variable per line
    - Correct:
        ```cs
            string firstName;
            string lastName;
        ```
    - Incorrect:
        ```cs
            string firstName, lastName;
        ```
- 12 Comments begin with an uppercase letter and end with a period
    - Correct:\
        // Code comment.
    - Incorrect:\
        // code comment
- 13 Insert one space between a comment delimiter and the comment
    - Correct:\
        // Code comment.
    - Incorrect:\
        //Code comment.
- 14 Each source file should only contain one class
- 15 There should be exactly one line between each method

### SQL
- General
    - 16 SQL keywords will be uppercase
        - Correct: SELECT * FROM table
        - Incorrect: select * FROM table
    - 17 Whenever a name is set, such as the name of a table, column, stored procedure, etc…, it will be in snake case
        - Correct: start_date, first_name, mineral, user_id
        - Incorrect: StartDate, firstName, FIRST_NAME
    - 18 When using a number in a name do not type the word
        - Correct: alternative_contact_1, alternative_contact_2
        - Incorrect: alternative_contact_one, alternative_contact_two
- Tables
    - 19 Table names do not have a prefix
        - Correct: minerals
        - Incorrect: tbl_minerals
    - 20 Prioritize naming tables using a collective when possible, otherwise use plural form 
        - staff, minerals, questions, answers
- Columns
    - 21 Column names are always in singular form
        - Correct: user_id
        - Incorrect: user_ids
    - 22 Always use the AS keyword when aliasing columns
        - Correct: mineral_name AS mn
        - Incorrect: mineral_name mn
- Stored procedures
    - 23 Stored procedures will be written with the format table_action
        - Correct: mineral_insert, user_delete
        - Incorrect: insert_mineral, delete_user
    - 24 Stored procedures do not have a prefix
        - Correct: user_insert
        - Incorrect: sp_user_insert
- Comments
    - 25 Single line comments use -- syntax
        - SELECT * FROM table -- selects everything from table
    - 26 Multi line comments use /* */ syntax
        - /*\
        comment line 1\
        comment line 2\
        comment line 3\
        */
