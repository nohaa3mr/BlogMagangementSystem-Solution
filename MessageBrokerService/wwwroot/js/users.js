// Users Page JavaScript
class UsersManager {
    constructor() {
        this.users = [];
        this.currentUser = null;
        this.init();
    }

    async init() {
        await this.loadUsers();
        this.setupEventListeners();
    }

    setupEventListeners() {
        // Create User Button
        document.getElementById('createUserBtn')?.addEventListener('click', () => {
            this.showCreateModal();
        });

        // Modal Close Buttons
        document.querySelectorAll('.modal-close').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const modal = e.target.closest('.modal');
                if (modal) modal.classList.remove('show');
            });
        });

        // Create User Form
        document.getElementById('createUserForm')?.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleCreateUser();
        });

        // Edit User Form
        document.getElementById('editUserForm')?.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleUpdateUser();
        });
    }

    async loadUsers() {
        try {
            const loadingDiv = document.getElementById('loading');
            if (loadingDiv) loadingDiv.style.display = 'block';

            this.users = await ApiService.getUsers();
            this.renderUsers();

            if (loadingDiv) loadingDiv.style.display = 'none';
        } catch (error) {
            Utils.showAlert('Failed to load users: ' + error.message, 'error');
            console.error(error);
        }
    }

    renderUsers() {
        const tbody = document.querySelector('#usersTable tbody');
        if (!tbody) return;

        if (this.users.length === 0) {
            tbody.innerHTML = '<tr><td colspan="6" class="empty-state">No users found. Create your first user!</td></tr>';
            return;
        }

        tbody.innerHTML = this.users.map(user => `
            <tr>
                <td>${user.email}</td>
                <td>${user.username}</td>
                <td>${user.fullName}</td>
                <td>${user.emailVerified ? '<span class="badge badge-success">Verified</span>' : '<span class="badge badge-danger">Not Verified</span>'}</td>
                <td>${Utils.formatDate(user.createdAt)}</td>
                <td class="action-buttons">
                    <button class="btn btn-sm btn-primary" onclick="usersManager.showEditModal('${user.id}')">Edit</button>
                    ${!user.emailVerified ? `<button class="btn btn-sm btn-success" onclick="usersManager.verifyEmail('${user.id}')">Verify Email</button>` : ''}
                    <button class="btn btn-sm btn-danger" onclick="usersManager.deleteUser('${user.id}')">Delete</button>
                </td>
            </tr>
        `).join('');
    }

    showCreateModal() {
        document.getElementById('createUserForm').reset();
        Utils.openModal('createUserModal');
    }

    showEditModal(userId) {
        const user = this.users.find(u => u.id === userId);
        if (!user) return;

        this.currentUser = user;
        document.getElementById('editUserId').value = user.id;
        document.getElementById('editEmail').value = user.email;
        document.getElementById('editUsername').value = user.username;
        document.getElementById('editFullName').value = user.fullName;
        document.getElementById('editBio').value = user.bio || '';

        Utils.openModal('editUserModal');
    }

    async handleCreateUser() {
        const form = document.getElementById('createUserForm');
        const formData = new FormData(form);

        const user = {
            email: formData.get('email'),
            username: formData.get('username'),
            fullName: formData.get('fullName'),
            bio: formData.get('bio') || ''
        };

        try {
            await ApiService.createUser(user);
            Utils.showAlert('User created successfully!', 'success');
            Utils.closeModal('createUserModal');
            await this.loadUsers();
        } catch (error) {
            Utils.showAlert('Failed to create user: ' + error.message, 'error');
        }
    }

    async handleUpdateUser() {
        const form = document.getElementById('editUserForm');
        const formData = new FormData(form);
        const userId = formData.get('userId');

        const user = {
            email: formData.get('email'),
            username: formData.get('username'),
            fullName: formData.get('fullName'),
            bio: formData.get('bio') || ''
        };

        try {
            await ApiService.updateUser(userId, user);
            Utils.showAlert('User updated successfully!', 'success');
            Utils.closeModal('editUserModal');
            await this.loadUsers();
        } catch (error) {
            Utils.showAlert('Failed to update user: ' + error.message, 'error');
        }
    }

    async deleteUser(userId) {
        if (!confirm('Are you sure you want to delete this user?')) return;

        try {
            await ApiService.deleteUser(userId);
            Utils.showAlert('User deleted successfully!', 'success');
            await this.loadUsers();
        } catch (error) {
            Utils.showAlert('Failed to delete user: ' + error.message, 'error');
        }
    }

    async verifyEmail(userId) {
        try {
            await ApiService.verifyUserEmail(userId);
            Utils.showAlert('Email verification event published!', 'success');
            await this.loadUsers();
        } catch (error) {
            Utils.showAlert('Failed to verify email: ' + error.message, 'error');
        }
    }
}

// Initialize when page loads
let usersManager;
document.addEventListener('DOMContentLoaded', () => {
    usersManager = new UsersManager();
});

