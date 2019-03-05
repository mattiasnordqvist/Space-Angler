# Space Angler

Based on knowledge from http://mikehillyer.com/articles/managing-hierarchical-data-in-mysql/, adapter for SQL Server.

## What is it?

This repo contain TSQL-scripts for adding Right and Left columns to an existing table with an hierarchical structure (no circular dependencies allowed!!). It also contain scripts for filling these new columns with values according to existing structure, and triggers to keep the values in sync when updating, inserting and deleting nodes from the hierarchical structure.

## How to use?

You should already have a table looking something like this:

| Id (PK) | Parent_Id (FK) | Other columns ... |
| ------- | -------------- | ----------------- |
|      1  |           NULL | ...               |
|      2  |              1 |                   |
|      3  |              1 |                   |
|      4  |              1 |                   |
|      5  |              2 |                   |
|      6  |           NULL |                   |

By running the included CLI, you're prompted to give (they can aslo be passed as parameters) the name of your table (-t), id column (-i) and parent id column (-p). The CLI will produce a script in out.sql. Running this script on your database would change your table to something like this:

| Id (PK) | Parent_Id (FK) | L  | R  | IsLeaf | Other columns ... |
| ------- | -------------- | -- | -- | -------| ----------------- |
|      1  |           NULL | 1  | 10 |      0 | ...               |
|      2  |              1 | 2  | 5  |      0 |                   |
|      3  |              1 | 6  | 7  |      1 |                   |
|      4  |              1 | 8  | 9  |      1 |                   |
|      5  |              2 | 3  | 4  |      1 |                   |
|      6  |           NULL | 11 | 12 |      1 |                   |

The script has also added a "after update"-trigger, a "instead of delete"-trigger and a "after insert"-trigger.

Now you can start indexing L and R, and query the structure in O(1) instead of O(logn)/O(n) (best and worst case using recursive cte)

Try it out!
