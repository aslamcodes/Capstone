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

# Frontend correction
- [x] Price tiers should be listed![[Pasted image 20240801143318.png]]
- [x] Adding a objective with a field value testing![[Pasted image 20240801143430.png]]
- [x] No responses to save button on course info edit, its better if save button is a floating action![[Pasted image 20240801143519.png]]
- Toggling between tabs, and sections I added vanishes, only returned after refreshing
- Course level not showing even after saving  ![[Pasted image 20240801143839.png]]
- Status not updating after save press  ![[Pasted image 20240801144102.png]]
- ![[Pasted image 20240801144155.png]] - Course Descirpiont page hero ![[Pasted image 20240801144326.png]]
- -Paying for order not repsonsive ![[Pasted image 20240801144352.png]]
- Initial course page is this for student, should there be something helpful![[Pasted image 20240801144435.png]]
- Video Icons for article on sectiondrops![[Pasted image 20240801144518.png]]
- Loggout isnt affecting course page![[Pasted image 20240801144554.png]]
- Course description![[Pasted image 20240801144613.png]]
- Search results and this weird description  ![[Pasted image 20240801144705.png]]\
- Clicking on search results doesnt navigate
- No way knowing if a user bought a course, there should be endpoint
- Courses on review is displayed and not with the profile picture
- ![[Pasted image 20240801144905.png]]