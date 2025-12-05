// API Service - Centralized API calls
const API_BASE_URL = '/api';

class ApiService {
    static async request(endpoint, options = {}) {
        const url = `${API_BASE_URL}${endpoint}`;
        const config = {
            headers: {
                'Content-Type': 'application/json',
                ...options.headers
            },
            ...options
        };

        if (config.body && typeof config.body === 'object') {
            config.body = JSON.stringify(config.body);
        }

        try {
            const response = await fetch(url, config);
            
            if (!response.ok) {
                const error = await response.text();
                throw new Error(error || `HTTP error! status: ${response.status}`);
            }

            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                return await response.json();
            }
            
            return await response.text();
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    }

    // User APIs
    static async getUsers() {
        return this.request('/users');
    }

    static async getUser(id) {
        return this.request(`/users/${id}`);
    }

    static async createUser(user) {
        return this.request('/users', {
            method: 'POST',
            body: user
        });
    }

    static async updateUser(id, user) {
        return this.request(`/users/${id}`, {
            method: 'PUT',
            body: user
        });
    }

    static async deleteUser(id) {
        return this.request(`/users/${id}`, {
            method: 'DELETE'
        });
    }

    static async verifyUserEmail(id) {
        return this.request(`/users/${id}/verify-email`, {
            method: 'POST'
        });
    }

    // Blog Post APIs
    static async getBlogPosts() {
        return this.request('/blogposts');
    }

    static async getBlogPost(id) {
        return this.request(`/blogposts/${id}`);
    }

    static async createBlogPost(post) {
        return this.request('/blogposts', {
            method: 'POST',
            body: post
        });
    }

    static async updateBlogPost(id, post) {
        return this.request(`/blogposts/${id}`, {
            method: 'PUT',
            body: post
        });
    }

    static async deleteBlogPost(id) {
        return this.request(`/blogposts/${id}`, {
            method: 'DELETE'
        });
    }

    static async publishBlogPost(id) {
        return this.request(`/blogposts/${id}/publish`, {
            method: 'POST'
        });
    }

    // Comment APIs
    static async getComments(postId = null) {
        const endpoint = postId ? `/comments?postId=${postId}` : '/comments';
        return this.request(endpoint);
    }

    static async getComment(id) {
        return this.request(`/comments/${id}`);
    }

    static async createComment(comment) {
        return this.request('/comments', {
            method: 'POST',
            body: comment
        });
    }

    static async updateComment(id, comment) {
        return this.request(`/comments/${id}`, {
            method: 'PUT',
            body: comment
        });
    }

    static async deleteComment(id) {
        return this.request(`/comments/${id}`, {
            method: 'DELETE'
        });
    }
}

// Utility Functions
const Utils = {
    showAlert(message, type = 'info') {
        const alertDiv = document.createElement('div');
        alertDiv.className = `alert alert-${type}`;
        alertDiv.textContent = message;
        
        const container = document.querySelector('.container') || document.body;
        container.insertBefore(alertDiv, container.firstChild);
        
        setTimeout(() => {
            alertDiv.remove();
        }, 5000);
    },

    formatDate(dateString) {
        if (!dateString) return 'N/A';
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    },

    generateSlug(text) {
        return text
            .toLowerCase()
            .trim()
            .replace(/[^\w\s-]/g, '')
            .replace(/[\s_-]+/g, '-')
            .replace(/^-+|-+$/g, '');
    },

    closeModal(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            modal.classList.remove('show');
        }
    },

    openModal(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            modal.classList.add('show');
        }
    }
};

