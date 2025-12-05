// Blog Posts Page JavaScript
class BlogPostsManager {
    constructor() {
        this.posts = [];
        this.users = [];
        this.currentPost = null;
        this.init();
    }

    async init() {
        await this.loadUsers();
        await this.loadPosts();
        this.setupEventListeners();
    }

    async loadUsers() {
        try {
            this.users = await ApiService.getUsers();
            this.populateAuthorSelects();
        } catch (error) {
            console.error('Failed to load users:', error);
        }
    }

    populateAuthorSelects() {
        const selects = document.querySelectorAll('select[name="authorId"]');
        selects.forEach(select => {
            select.innerHTML = '<option value="">Select Author</option>' +
                this.users.map(user => 
                    `<option value="${user.id}">${user.fullName} (${user.email})</option>`
                ).join('');
        });
    }

    setupEventListeners() {
        document.getElementById('createPostBtn')?.addEventListener('click', () => {
            this.showCreateModal();
        });

        document.querySelectorAll('.modal-close').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const modal = e.target.closest('.modal');
                if (modal) modal.classList.remove('show');
            });
        });

        document.getElementById('createPostForm')?.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleCreatePost();
        });

        document.getElementById('editPostForm')?.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleUpdatePost();
        });

        // Auto-generate slug from title
        document.getElementById('createTitle')?.addEventListener('input', (e) => {
            const slugInput = document.getElementById('createSlug');
            if (slugInput && !slugInput.value) {
                slugInput.value = Utils.generateSlug(e.target.value);
            }
        });

        document.getElementById('editTitle')?.addEventListener('input', (e) => {
            const slugInput = document.getElementById('editSlug');
            if (slugInput) {
                slugInput.value = Utils.generateSlug(e.target.value);
            }
        });
    }

    async loadPosts() {
        try {
            const loadingDiv = document.getElementById('loading');
            if (loadingDiv) loadingDiv.style.display = 'block';

            this.posts = await ApiService.getBlogPosts();
            this.renderPosts();

            if (loadingDiv) loadingDiv.style.display = 'none';
        } catch (error) {
            Utils.showAlert('Failed to load posts: ' + error.message, 'error');
            console.error(error);
        }
    }

    renderPosts() {
        const tbody = document.querySelector('#postsTable tbody');
        if (!tbody) return;

        if (this.posts.length === 0) {
            tbody.innerHTML = '<tr><td colspan="6" class="empty-state">No blog posts found. Create your first post!</td></tr>';
            return;
        }

        tbody.innerHTML = this.posts.map(post => {
            const author = this.users.find(u => u.id === post.authorId);
            return `
                <tr>
                    <td>${post.title}</td>
                    <td>${author ? author.fullName : 'Unknown'}</td>
                    <td>${post.category || 'Uncategorized'}</td>
                    <td>${post.isPublished ? '<span class="badge badge-success">Published</span>' : '<span class="badge badge-warning">Draft</span>'}</td>
                    <td>${Utils.formatDate(post.createdAt)}</td>
                    <td class="action-buttons">
                        <button class="btn btn-sm btn-primary" onclick="blogPostsManager.showEditModal('${post.id}')">Edit</button>
                        ${!post.isPublished ? `<button class="btn btn-sm btn-success" onclick="blogPostsManager.publishPost('${post.id}')">Publish</button>` : ''}
                        <button class="btn btn-sm btn-danger" onclick="blogPostsManager.deletePost('${post.id}')">Delete</button>
                    </td>
                </tr>
            `;
        }).join('');
    }

    showCreateModal() {
        document.getElementById('createPostForm').reset();
        this.populateAuthorSelects();
        Utils.openModal('createPostModal');
    }

    showEditModal(postId) {
        const post = this.posts.find(p => p.id === postId);
        if (!post) return;

        this.currentPost = post;
        document.getElementById('editPostId').value = post.id;
        document.getElementById('editTitle').value = post.title;
        document.getElementById('editSlug').value = post.slug;
        document.getElementById('editContent').value = post.content;
        document.getElementById('editCategory').value = post.category || '';
        document.getElementById('editExcerpt').value = post.excerpt || '';
        document.getElementById('editAuthorId').value = post.authorId;
        document.getElementById('editIsPublished').checked = post.isPublished;

        this.populateAuthorSelects();
        Utils.openModal('editPostModal');
    }

    async handleCreatePost() {
        const form = document.getElementById('createPostForm');
        const formData = new FormData(form);

        const post = {
            authorId: formData.get('authorId'),
            title: formData.get('title'),
            slug: formData.get('slug'),
            content: formData.get('content'),
            category: formData.get('category') || '',
            excerpt: formData.get('excerpt') || '',
            isPublished: formData.get('isPublished') === 'on'
        };

        try {
            await ApiService.createBlogPost(post);
            Utils.showAlert('Blog post created successfully!', 'success');
            Utils.closeModal('createPostModal');
            await this.loadPosts();
        } catch (error) {
            Utils.showAlert('Failed to create post: ' + error.message, 'error');
        }
    }

    async handleUpdatePost() {
        const form = document.getElementById('editPostForm');
        const formData = new FormData(form);
        const postId = formData.get('postId');

        const post = {
            authorId: formData.get('authorId'),
            title: formData.get('title'),
            slug: formData.get('slug'),
            content: formData.get('content'),
            category: formData.get('category') || '',
            excerpt: formData.get('excerpt') || '',
            isPublished: formData.get('isPublished') === 'on'
        };

        try {
            await ApiService.updateBlogPost(postId, post);
            Utils.showAlert('Blog post updated successfully!', 'success');
            Utils.closeModal('editPostModal');
            await this.loadPosts();
        } catch (error) {
            Utils.showAlert('Failed to update post: ' + error.message, 'error');
        }
    }

    async deletePost(postId) {
        if (!confirm('Are you sure you want to delete this post?')) return;

        try {
            await ApiService.deleteBlogPost(postId);
            Utils.showAlert('Post deleted successfully!', 'success');
            await this.loadPosts();
        } catch (error) {
            Utils.showAlert('Failed to delete post: ' + error.message, 'error');
        }
    }

    async publishPost(postId) {
        try {
            await ApiService.publishBlogPost(postId);
            Utils.showAlert('Post published successfully! Event published!', 'success');
            await this.loadPosts();
        } catch (error) {
            Utils.showAlert('Failed to publish post: ' + error.message, 'error');
        }
    }
}

let blogPostsManager;
document.addEventListener('DOMContentLoaded', () => {
    blogPostsManager = new BlogPostsManager();
});

