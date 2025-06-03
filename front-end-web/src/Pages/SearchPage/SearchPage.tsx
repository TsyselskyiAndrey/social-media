import React, { useState, useEffect, useRef } from "react";
import { Box, CssBaseline, TextField, InputAdornment, Typography, Chip } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import LeftSidebar from '../../Components/MainPageComponents/LeftSidebar/LeftSidebar';
import Agent from "../../API/agent";
import { Tag, Post } from "../../API/agent";
import "./SearchPage.css";
import { useToast } from '../../Contexts/ToastContext';
import PostModal from '../../Components/ProfilePageComponents/PostGridTypes/PostModal';

interface User {
  id: number;
  name: string;
  handle: string;
  avatar: string;
}

const SearchPage: React.FC = () => {

  const { showSuccess, showError, showWarning, showInfo, showZeroPosts } = useToast();
  
  const [searchQuery, setSearchQuery] = useState("");
  const [tags, setTags] = useState<Tag[]>([]);
  const [selectedTag, setSelectedTag] = useState<Tag | null>(null);
  const [posts, setPosts] = useState<Post[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [showUserResults, setShowUserResults] = useState(false);
  const [isAnimatingOut, setIsAnimatingOut] = useState(false);
  const [popularTags, setPopularTags] = useState<Tag[]>([]);
  const searchRef = useRef<HTMLDivElement>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedPost, setSelectedPost] = useState<Post | null>(null);

  // Заглушка для користувачів (в реальному проекті API виклик)
  const mockUsers = [
    { id: 1, name: 'Ірина Сидоренко', handle: '@iryna', avatar: 'І' },
    { id: 2, name: 'Андрій Коваленко', handle: '@andrii', avatar: 'А' },
    { id: 3, name: 'Марія Литвин', handle: '@maria', avatar: 'М' },
    { id: 4, name: 'Олександр Петренко', handle: '@oleksandr', avatar: 'О' },
    { id: 5, name: 'Наталія Шевченко', handle: '@natalia', avatar: 'Н' },
  ];

  const [filteredUsers, setFilteredUsers] = useState<User[]>([]);

  const openModal = (post: Post) => {
    setSelectedPost(post);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setSelectedPost(null);
  };

  const updatePostInList = (updatedPost: Post) => {
    setPosts(prevPosts => 
      prevPosts.map(post => 
        post.id === updatedPost.id ? updatedPost : post
      )
    );
  };

  useEffect(() => {
    const fetchTags = async () => {
      setIsLoading(true);
      try {
        const response = await Agent.Tags.getAllTags();
        setTags(response.data);
        
     
        const popular = response.data
          .sort(() => 0.5 - Math.random())
          .slice(0, 8);
        setPopularTags(popular);
      } catch (error) {
        console.error("Помилка завантаження тегів:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchTags();
    fetchPosts();
  }, []);

  const fetchPosts = async (tagIds?: number[]) => {
    setIsLoading(true);
    try {
      const response = await Agent.Posts.getPosts(
        null,
        20,
        null,
        tagIds || null,
        null
      );
      
      let filteredPosts = response.data;
      if (tagIds && tagIds.length > 0) {
        filteredPosts = response.data.filter(post => 
          post.tags && post.tags.some(tagName => {

            const matchingTag = tags.find(t => t.name === tagName);
      
            return matchingTag && tagIds.includes(matchingTag.id);
          })
        );
        

        setPosts(filteredPosts);

        const currentTagName = tags.find(t => t.id === tagIds[0])?.name;
        if (currentTagName) {
     
          if (filteredPosts.length === 0) {
            showZeroPosts(`Знайдено ${filteredPosts.length} постів з тегом #${currentTagName}`);
          } else {
            showSuccess(`Знайдено ${filteredPosts.length} постів з тегом #${currentTagName}`);
          }
        }
      } else {
        setPosts(response.data);
      }
    } catch (error) {
      console.error("Помилка завантаження постів:", error);
      showError("Помилка завантаження постів. Спробуйте ще раз.");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    if (searchQuery && !searchQuery.startsWith('#')) {
      const userResults = mockUsers.filter(user => 
        user.name.toLowerCase().includes(searchQuery.toLowerCase()) || 
        user.handle.toLowerCase().includes(searchQuery.toLowerCase())
      ).slice(0, 5); // Обмежуємо до 5 результатів
      setFilteredUsers(userResults);
      setShowUserResults(userResults.length > 0);
    } else {
      setFilteredUsers([]);
      setShowUserResults(false);
    }
  }, [searchQuery]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (searchRef.current && !searchRef.current.contains(event.target as Node)) {
        setShowUserResults(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const handleTagSelect = (tag: Tag) => {
    setSelectedTag(tag);
    setSearchQuery(`#${tag.name}`);
    fetchPosts([tag.id]);
    setShowUserResults(false);
  };

  const hideUserResults = () => {
    setIsAnimatingOut(true);
    setTimeout(() => {
      setShowUserResults(false);
      setIsAnimatingOut(false);
    }, 200);
  };

  const handleUserSelect = (userId: number) => {
    console.log(`Перехід на сторінку користувача з ID: ${userId}`);
    hideUserResults();
  };

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (searchRef.current && !searchRef.current.contains(event.target as Node)) {
        hideUserResults();
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const renderTagSuggestions = () => {
    if (!searchQuery || !searchQuery.startsWith('#')) return null;
    
    const query = searchQuery.substring(1).toLowerCase();
    const filteredTags = tags.filter(tag => 
      tag.name.toLowerCase().includes(query)
    ).slice(0, 5);

    if (filteredTags.length === 0) return null;

    return (
      <Box className="tag-suggestions">
        {filteredTags.map(tag => (
          <Box 
            key={tag.id} 
            className="tag-suggestion-item"
            onClick={() => handleTagSelect(tag)}
          >
            #{tag.name}
          </Box>
        ))}
      </Box>
    );
  };

  return (
    <>
      <CssBaseline />
      <Box
        sx={{
          display: "flex",
          maxWidth: 1800,
          mx: "auto",
          pt: 8,
          px: 2,
          gap: "70px",
          position: "relative",
        }}
      >
        <Box
          sx={{
            display: { xs: "none", md: "block" },
            flexBasis: { md: "25%", lg: "22%" },
            minWidth: 250,
          }}
        >
          <LeftSidebar onCreateClick={() => {}} />
        </Box>

        <Box
          sx={{
            flex: 1,
            flexBasis: { xs: "100%", md: "55%", lg: "56%" },
            minWidth: { xs: "100%", md: 700 },
            position: "relative",
            zIndex: 1,
          }}
        >
          <Box sx={{ mb: 3, position: "relative" }} ref={searchRef}>
            <Typography variant="h5" fontWeight="bold" sx={{ mb: 2 }}>
              Пошук
            </Typography>
            <TextField
              fullWidth
              placeholder="Шукати користувачів або #теги"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon fontSize="large" />
                  </InputAdornment>
                ),
              }}
              className="search-bar"
            />

            {showUserResults && filteredUsers.length > 0 && (
              <Box className="user-results-dropdown">
                {filteredUsers.map(user => (
                  <Box 
                    key={user.id} 
                    className="user-result-item"
                    onClick={() => handleUserSelect(user.id)}
                  >
                    <Box className="user-avatar">{user.avatar}</Box>
                    <Box className="user-info">
                      <Typography variant="body1" className="user-name">{user.name}</Typography>
                      <Typography variant="body2" className="user-handle">{user.handle}</Typography>
                    </Box>
                  </Box>
                ))}
              </Box>
            )}

            {renderTagSuggestions()}

            <Box className="search-tags-container">
              {popularTags.map(tag => (
                <Chip
                  key={tag.id}
                  label={`#${tag.name}`}
                  onClick={() => handleTagSelect(tag)}
                  className="tag-chip"
                  color={selectedTag?.id === tag.id ? "primary" : "default"}
                  variant={selectedTag?.id === tag.id ? "filled" : "outlined"}
                />
              ))}
            </Box>
          </Box>

          {selectedTag && (
            <Box sx={{ mb: 3 }}>
              <Typography variant="h6" fontWeight="bold">
                #{selectedTag.name}
              </Typography>
            </Box>
          )}

          <Box className="posts-grid" sx={{ minHeight: '300px', width: '100%' }}>            
            {isLoading ? (
              <Box sx={{ textAlign: 'center', py: 4 }}>
                <Typography>Завантаження...</Typography>
              </Box>
            ) : posts.length > 0 ? (
              <div className="grid grid-cols-3 gap-1 md:gap-2">
                {posts.map((post) => (
                  <div 
                    key={post.id} 
                    className="aspect-square overflow-hidden bg-gray-100 rounded-md cursor-pointer hover:opacity-80 transition-opacity"
                    onClick={() => openModal(post)}
                  >
                    {post.postMedias && post.postMedias.length > 0 && (
                      post.postMedias[0].format.startsWith('video') ? (
                        <div className="relative w-full h-full">
                          <video 
                            src={post.postMedias[0].mediaUrl} 
                            className="object-cover w-full h-full"
                            poster={post.postMedias[0].thumbnailUrl || undefined}
                          />
                          <div className="absolute top-2 right-2 bg-black bg-opacity-50 text-white rounded-full p-1">
                            <span className="material-icons">play_arrow</span>
                          </div>
                        </div>
                      ) : (
                        <img
                          src={post.postMedias[0].mediaUrl}
                          alt={`post-${post.id}`}
                          className="object-cover w-full h-full hover:scale-105 transition-transform duration-200"
                          loading="lazy"
                        />
                      )
                    )}
                  </div>
                ))}
              </div>
            ) : (
              <Box sx={{ 
                textAlign: 'center', 
                py: 4, 
                display: 'flex', 
                justifyContent: 'center', 
                alignItems: 'center', 
                height: '300px',
                width: '100%',
                border: '1px dashed #000',
                borderRadius: '8px'
              }}>
                <Typography fontWeight="bold" variant="h6">Немає доступних постів</Typography>
              </Box>
            )}
          </Box>
        </Box>
      </Box>
      
      {selectedPost && (
        <PostModal
          isOpen={modalOpen}
          onClose={closeModal}
          post={selectedPost}
          onPostUpdate={updatePostInList}
        />
      )}
    </>
  );
};

export default SearchPage;