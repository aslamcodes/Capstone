### Feature Development Flow: TDD
- Define Service's Interfaces and its methods
- Write Test cases and make it fail
- Implement the services until all test cases passes
- Complete the flow
### Getting into a course
**Use Case: Enroll in Course**
**Actors:** User, System
**Preconditions:** User is logged in and has access to course enrollment options.
**Basic Flow:**
1. **User selects course** from the available options.
2. **System presents two options:** "Enroll" or "Get it with subscription."
3. **If "Enroll" is selected:**
    - System creates an order for the course.
    - System directs the user to the orders page.
    - User completes payment for the order.
    - System adds the course to the user's course list.
4. **If "Get it with subscription" is selected:**
    - System checks the user's subscription status.
    - If subscription is valid, System adds the course to the user's course list.
**Postconditions:** The user is enrolled in the course and can access course materials.

### Use Case: Course Submission Process
**Actors:** Educator, Admin Team, System
**Preconditions:**
- Educator is logged in.
- Educator has access to the course creation tools.
**Basic Flow:**
1. **Educator creates a course** using the provided tools.
2. **Educator submits** the course for review.
3. **Admin team verifies** the course content and quality.
4. **Admin team approves** the course.
5. **System makes the course live** and available for enrollment.
**Postconditions:** The course is live and available for students to enroll.

### Course Validity check
- [ ] Should have atleast 4 sections
- [ ] Should have 16 contents atleast
- [ ] Should have a pricing tier selected
- [ ] 
# Udemy - User Research
My main complaint would be that they never remove dead courses. There are many courses that someone put up years ago, never once updated, and that hasn't made any sales. I think it would be great if Udemy would contact those instructors to say "improve your course or we'll take it off the platform in 90 days." Of course instructors deserve the opportunity to re-design their courses, but dead courses with absent instructors make picking the right course harder for students, and can be a negative experience for the unlucky students who give them a try.

