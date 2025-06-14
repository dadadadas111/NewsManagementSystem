# News Management System - TODO

## 1. Details View
DONE

## 2. Create Functionality
### API
- DONE: Implement POST endpoints for creating NewsArticle, Category, Tag, and SystemAccount.
- DONE: Apply data type validation and model validation attributes (e.g., [Required], [StringLength], [Range]) for all DTO fields.
- DONE: Allow user-supplied TagId and AccountId for Tag and SystemAccount creation.
- DONE: Return appropriate error responses for invalid data.

### WebUI
- DONE: Add a user-friendly form for creating each entity (NewsArticle, Category, Tag, SystemAccount).
- DONE: Use proper input types, validation messages, and dropdowns for related entities.
- DONE: Allow user to specify TagId and AccountId for Tag and SystemAccount creation.
- DONE: Ensure the UI is modern, accessible, and responsive.

## 3. Update Functionality
### API
- DONE: Implement PUT endpoints for updating entities using Update DTOs.
- DONE: Apply the same validation as in create functionality.

### WebUI
- DONE: Add edit forms for each entity, pre-populated with existing data.
- DONE: Use the same UI/UX standards as the create forms.
- DONE: Sync edit forms with API and handle API validation errors.

## 4. Delete Functionality
### API
- DONE: Implement DELETE endpoints for all entities (NewsArticle, Category, Tag, SystemAccount).

### WebUI
- DONE: Add delete buttons and confirmation modals for all entities.
- DONE: Sync delete actions with API and update UI after deletion.

## 5. Authentication and Authorization
### API
- Add endpoints for user registration and login (e.g., /api/auth/register, /api/auth/login).
- Implement JWT-based authentication.
- Add role-based authorization to API endpoints (e.g., only Admin can create/update/delete, regular users can only view).
- Extract user role from JWT claims and enforce access control.

### WebUI
- Add a login screen that is required before accessing any other page.
- Store authentication token securely (e.g., in HttpOnly cookies or local storage).
- Show/hide UI elements based on user role (e.g., hide admin features for regular users).
- Implement logout functionality.

---

**All CRUD (Create, Read, Update, Delete) functionality is now complete and fully synchronized between API and WebUI.**

