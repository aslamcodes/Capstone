# User Entity
```mermaid
classDiagram
    class User {
        -UUID user_id
        +VARCHAR name
        +VARCHAR email
        -VARCHAR password_hash
        +ENUM role
        +TIMESTAMP created_at
        +TIMESTAMP updated_at
        +ENUM status
        +VARCHAR profile_picture
        +TEXT bio
        +TIMESTAMP last_login
    }

```
# Content
- It can be a `Article` or `Video`
```mermaid
classDiagram
class Content { 
-UUID content_id 
-UUID content_item_id 
+ENUM content_type (Aritcle, Video)
+INTEGER order_index 
}
```
## Video
```mermaid
classDiagram
    class Video {
        -UUID video_id
        -UUID course_id
        +VARCHAR title
        +VARCHAR url
        +TIMESTAMP created_at
        +TIMESTAMP updated_at
        +INTEGER duration
        +TEXT description
        +ENUM status
        +VARCHAR thumbnail_url
        +INTEGER views
    }
```
## Article
```mermaid
classDiagram
class Article {
-UUID article_id 
+VARCHAR title 
+TEXT content 
+TIMESTAMP created_at 
+TIMESTAMP updated_at 
+ENUM status 
}
```

# Course
```mermaid
classDiagram
class Course { 
-UUID course_id
+VARCHAR title
+TEXT description
-UUID educator_id
+DECIMAL price 
+ ENUM CourseLevel
}
```
# CourseCategory and CourseSkills
```mermaid
classDiagram
class CourseCategory {
uuid courseId,
uuid categoryId
}
class CourseSkills {
uuid id,
uuid skillId,
uuid courseId
}
```
# Course Objectives, Target Audience & Prerequisites
```mermaid
classDiagram 
class CourseObjectives {
uuid id,
uuid courseId,
int order,
text objective
}

class TargetAudience {
uuid id,
uuid courseId,
int order,
text audience
}

class Prerequisites {
uuid id, 
uuid courseId,
int order,
text requiesite
}
```
# Section
- Groups of content, helps to organise courses effectively
```mermaid
classDiagram 
class Section {
UUID id,
UUID courseid,
INT OrderId,
TEXT title,
TEXT Description
}
```

## `CourseSection`
Bride between Section and Courses
```mermaid
classDiagram 
class CourseSection {
 UUID id,
 UUID courseId,
 UUID sectionId,
 UUID orderId
}
```
### Section Content
- A Bride between section and series of content
- A `Section` might have many `CourseContent` of different types
```mermaid
classDiagram
class sectionContent { 
-UUID ID
-UUID sectionId 
-UUID content_id 
+INTEGER order_index 
}
```
# Learning Paths
- For example, `MERN` dev learning path would be comprised of series of courses that the user might have to take to become a solid developer
- It might start with courses like `Getting into Web dev`, Followed by `React`, `NodeJS` and `MongoDB`
- `LearningPathCourse` bride between `LearningPath` and series of `Course`s
- `LearningPath` might contain 1 to many `courses`
```mermaid
classDiagram
class LearningPath {
    -UUID learning_path_id
    +VARCHAR title
    +TEXT description
    +TIMESTAMP created_at
    +TIMESTAMP updated_at
    +ENUM status
    +ENUM Category
    +ENUM[] tags
}

```
```mermaid
classDiagram
class LearningPathCourse {
    -UUID id
    -UUID learning_path_id
    -UUID course_id
    +INTEGER order_index
}
```
# Subscriptions
- With Subscription, according to business logic user can access set of courses are whole of courses
```mermaid
classDiagram
class Subscription {
-UUID uuid
-UUID userUuid
+TIMESTAMP subscriptionStart 
+TIMESTAMP subscriptionEnd 
+ENUM subscriptionType
}
```
# Notes
- A `note` has to be related to a `content`
- That way notes can be gathered together with contents as an overview note
```mermaid
classDiagram 
class Note { 
-UUID note_id 
-UUID content_id 
-UUID user_id
+TEXT note_text 
+TIMESTAMP created_at
+TIMESTAMP updated_at }
```

# Review
- Reviews by students that are taking a particular course
```mermaid
classDiagram
class Review {
UUID id,
TEXT review,
TIMESTAMP createdAt,
INT stars 
UUID userId,
UUID courseId,
}
```
# Questions & Answers
Questions are related to individual `content`, can be a video, article
```mermaid
classDiagram
class Question {
UUID id,
UUID contentId,
TIMESTAMP createdAt,
TEXT questionText,
UUID userId,
INT votes
}

class Answer {
UUID id,
UUID questionId,
UUID userId,
TIMESTAMP createdAt,
INT votes,
TEXT answerText
}
```

# Course Category and Skills
```mermaid
classDiagram
class Category {
uuid id,
text name,
text icon
}

class skill {
uuid id,
text name,
text icon
}
```