## Functional Requirements

1. **Course Management**
   - Educators can create and manage courses.
   - Courses are segmented into sections.
   - Each section can contain multiple pieces of content.
   - Sections and their content can be ordered and rearranged.
   - Courses can have many skills associated with them
   - Courses have single category
   - A Student can search for any courses with text input or category wise or skill wise

2. **Content Types**
   - Content within a section can be either an article or a video.
   - Content can have associated questions and answers.

3. **Course Details**
   - Each course should include course objectives, target audience, and prerequisites.
   - These details can be ordered and managed.

4. **Student Interaction**
   - Students can enroll in courses through subscriptions or one-time payments.
   - Students can take notes on any content they consume within a course.
   - Students can provide reviews for the courses they enroll in.
   - Students can opt in for learning tools such as reminder text messages
   - A student can select his interested areas, which is the tags of the courses or categories

5. **Admin Features**
   - Admins can create and manage learning paths, which are collections of curated courses.
   - Admins have to verify courses that are submitted for review

6. **Reviews**
   - Courses should have a review feature for students to provide feedback.

7. **User Management**
- A new user can register with the platform
- A user can be both educator and student
- A user cannot be admin and instructors, student at the same time
- A user can log in and log out

## Non-Functional Requirements

1. **Video Streaming**
	- Video content should be streamed to ensure a smooth viewing experience.
2. **User Interface**
	- UI should be reactive and responsive
	- UI should react to different user roles
3. **Security:**
   - Implement access controls to ensure that only authorized users can perform set of operations
4.  **Scalability:**
   - Design the system to accommodate the addition of new courses and its content without significant changes to the underlying architecture.