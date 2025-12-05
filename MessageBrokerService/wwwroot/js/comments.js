// Comments Page JavaScript
class CommentsManager {
    constructor() {
        this.comments = [];
        this.posts = [];
        this.currentComment = null;
        this.init();
    }

    async init() {
        await this.loadPosts();
        await this.loadComments();
        this.setupEventListeners();
    }

    async loadPosts() {
        try {
            this.posts = await ApiService.getBlogPosts();
            this.populatePostSelects();
        } catch (error) {
            console.error('Failed to load posts:', error);
        }
    }

    populatePostSelects() {
        const selects = document.querySelectorAll('select[name="postId"]');
        selects.forEach(select => {
            select.innerHTML = '<option value="">Select Post</option>' +
                this.posts.map(post => 
                    `<option value="${post.id}">${post.title}</option>`
                ).join('');
        });
    }

    setupEventListeners() {
        document.getElementById('createCommentBtn')?.addEventListener('click', () => {
            this.showCreateModal();
        });

        document.querySelectorAll('.modal-close').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const modal = e.target.closest('.modal');
                if (modal) modal.classList.remove('show');
            });
        });

        document.getElementById('createCommentForm')?.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleCreateComment();
        });

        document.getElementById('editCommentForm')?.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleUpdateComment();
        });
    }

    async loadComments() {
        try {
            const loadingDiv = document.getElementById('loading');
            if (loadingDiv) loadingDiv.style.display = 'block';

            this.comments = await ApiService.getComments();
            this.renderComments();

            if (loadingDiv) loadingDiv.style.display = 'none';
        } catch (error) {
            Utils.showAlert('Failed to load comments: ' + error.message, 'error');
            console.error(error);
        }
    }

    renderComments() {
        const tbody = document.querySelector('#commentsTable tbody');
        if (!tbody) return;

        if (this.comments.length === 0) {
            tbody.innerHTML = '<tr><td colspan="6" class="empty-state">No comments found. Create your first comment!</td></tr>';
            return;
        }

        tbody.innerHTML = this.comments.map(comment => {
            const post = this.posts.find(p => p.id === comment.postId);
            const contentPreview = comment.content.length > 50 
                ? comment.content.substring(0, 50) + '...' 
                : comment.content;
            
            return `
                <tr>
                    <td>${post ? post.title : 'Unknown Post'}</td>
                    <td>${comment.authorName || 'Anonymous'}</td>
                    <td>${contentPreview}</td>
                    <td>${comment.parentCommentId ? '<span class="badge badge-info">Reply</span>' : '<span class="badge badge-success">Comment</span>'}</td>
                    <td>${Utils.formatDate(comment.createdAt)}</td>
                    <td class="action-buttons">
                        <button class="btn btn-sm btn-primary" onclick="commentsManager.showEditModal('${comment.id}')">Edit</button>
                        <button class="btn btn-sm btn-danger" onclick="commentsManager.deleteComment('${comment.id}')">Delete</button>
                    </td>
                </tr>
            `;
        }).join('');
    }

    showCreateModal() {
        document.getElementById('createCommentForm').reset();
        this.populatePostSelects();
        Utils.openModal('createCommentModal');
    }

    showEditModal(commentId) {
        const comment = this.comments.find(c => c.id === commentId);
        if (!comment) return;

        this.currentComment = comment;
        document.getElementById('editCommentId').value = comment.id;
        document.getElementById('editContent').value = comment.content;
        document.getElementById('editPostId').value = comment.postId;
        document.getElementById('editAuthorName').value = comment.authorName;
        document.getElementById('editAuthorEmail').value = comment.authorEmail;

        this.populatePostSelects();
        Utils.openModal('editCommentModal');
    }

    async handleCreateComment() {
        const form = document.getElementById('createCommentForm');
        const formData = new FormData(form);

        const comment = {
            postId: formData.get('postId'),
            authorName: formData.get('authorName') || '',
            authorEmail: formData.get('authorEmail') || '',
            content: formData.get('content'),
            parentCommentId: formData.get('parentCommentId') || null
        };

        try {
            await ApiService.createComment(comment);
            Utils.showAlert('Comment created successfully! Event published!', 'success');
            Utils.closeModal('createCommentModal');
            await this.loadComments();
        } catch (error) {
            Utils.showAlert('Failed to create comment: ' + error.message, 'error');
        }
    }

    async handleUpdateComment() {
        const form = document.getElementById('editCommentForm');
        const formData = new FormData(form);
        const commentId = formData.get('commentId');

        const comment = {
            content: formData.get('content')
        };

        try {
            await ApiService.updateComment(commentId, comment);
            Utils.showAlert('Comment updated successfully!', 'success');
            Utils.closeModal('editCommentModal');
            await this.loadComments();
        } catch (error) {
            Utils.showAlert('Failed to update comment: ' + error.message, 'error');
        }
    }

    async deleteComment(commentId) {
        if (!confirm('Are you sure you want to delete this comment?')) return;

        try {
            await ApiService.deleteComment(commentId);
            Utils.showAlert('Comment deleted successfully!', 'success');
            await this.loadComments();
        } catch (error) {
            Utils.showAlert('Failed to delete comment: ' + error.message, 'error');
        }
    }
}

let commentsManager;
document.addEventListener('DOMContentLoaded', () => {
    commentsManager = new CommentsManager();
});

