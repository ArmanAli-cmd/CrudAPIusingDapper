# CrudAPIusingDapper

## Many to Many Realtionships
- One Student can enrolled in multiple courses and One course can be enrolled by many students that means multiple students can enrolled many courses so that's why it's called many to many relationship.
- Here StudentId & CourseId in StudentCourse table use the foreign key concept for both table (Student & Course).

### Student Table
| Column Name | Description |
| --- | --- |
| StudentId | Primary key |
| StudentName | Name of students |
| StudentHome | City name of students |

### Course Table
| Column Name | Description |
| --- | --- |
| CourseId | Primary key |
| CourseName | Name of courses |
| Price | Price of courses |

### StudentCourse Table
| Column Name | Description |
| --- | --- |
| StudentId | Foreign key |
| CourseId | Foreign key |
| Price | Price of courses |
| DateTime | Date & Time when students enrolled in a course |
