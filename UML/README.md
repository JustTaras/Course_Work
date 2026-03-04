Це UML діаграма для Mermaid

<img width="6743" height="2456" alt="mermaid-diagram-2026-03-04-140903" src="https://github.com/user-attachments/assets/4c4884d8-0093-44fa-aed7-79e6f50a2d0c" />

classDiagram

%% =========================
%%        MODELS
%% =========================

class User {
    -int id
    -string name
    -string email
    +getInfo() string
}

class Student {
    +viewAssignments() List~Assignment~
    +submitAssignment(assignmentId, content)
}

class Teacher {
    +createAssignment(title, description, deadline, groupId)
    +checkSubmission(submissionId)
    +gradeSubmission(submissionId, grade)
}

class Assignment {
    -int id
    -string title
    -string description
    -DateTime deadline
    -int groupId
    +isExpired() bool
    +getInfo() string
}

class Submission {
    -int id
    -int assignmentId
    -int studentId
    -string content
    -DateTime submitDate
    -int grade
    -string status
    +setGrade(grade)
    +changeStatus(status)
}

class Group {
    -int id
    -string name
    -List~Student~ students
    +addStudent(student)
    +removeStudent(student)
}

%% =========================
%%     REPOSITORIES
%% =========================

class IUserRepository {
    <<interface>>
    +addUser(user)
    +getUserById(id) User
    +getAllUsers() List~User~
}

class IAssignmentRepository {
    <<interface>>
    +addAssignment(assignment)
    +getById(id) Assignment
    +getAll() List~Assignment~
}

class ISubmissionRepository {
    <<interface>>
    +addSubmission(submission)
    +getById(id) Submission
    +getByAssignmentId(id) List~Submission~
}

class IGroupRepository {
    <<interface>>
    +addGroup(group)
    +getById(id) Group
    +getAll() List~Group~
}

class InMemoryRepository {
    -List~User~ users
    -List~Assignment~ assignments
    -List~Submission~ submissions
    -List~Group~ groups
}

%% =========================
%%      UNIT OF WORK
%% =========================

class IUnitOfWork {
    <<interface>>
    +IUserRepository Users
    +IAssignmentRepository Assignments
    +ISubmissionRepository Submissions
    +IGroupRepository Groups
    +commit()
}

class UnitOfWork {
    -IUserRepository userRepo
    -IAssignmentRepository assignmentRepo
    -ISubmissionRepository submissionRepo
    -IGroupRepository groupRepo
    +commit()
}

%% =========================
%%        SERVICES
%% =========================

class AssignmentService {
    -IUnitOfWork unitOfWork
    +createAssignment(title, description, deadline, groupId)
    +getAssignmentsByGroup(groupId)
    +getActiveAssignments()
}

class SubmissionService {
    -IUnitOfWork unitOfWork
    +submit(assignmentId, studentId, content)
    +check(submissionId)
    +grade(submissionId, grade)
}

class GroupService {
    -IUnitOfWork unitOfWork
    +createGroup(name)
    +addStudentToGroup(studentId, groupId)
}

%% =========================
%%     RELATIONSHIPS + MULTIPLICITY
%% =========================

User <|-- Student
User <|-- Teacher

Teacher "1" --> "0..*" Assignment : creates
Assignment "1" --> "0..*" Submission : has
Student "1" --> "0..*" Submission : submits

Group "1" o-- "0..*" Student : contains

AssignmentService --> "1" IUnitOfWork
SubmissionService --> "1" IUnitOfWork
GroupService --> "1" IUnitOfWork

IUserRepository <|.. InMemoryRepository
IAssignmentRepository <|.. InMemoryRepository
ISubmissionRepository <|.. InMemoryRepository
IGroupRepository <|.. InMemoryRepository

IUnitOfWork <|.. UnitOfWork
