import React from "react";

const dummyPosts = Array.from({ length: 12 }, (_, i) =>
  `https://picsum.photos/seed/post${i}/600/600`
);

const PostsTab: React.FC = () => (
  <div className="grid grid-cols-3 gap-1 md:gap-2">
    {dummyPosts.map((src, index) => (
      <div key={index} className="aspect-square overflow-hidden bg-gray-100 rounded-md">
        <img
          src={src}
          alt={`post-${index}`}
          className="object-cover w-full h-full hover:scale-105 transition-transform duration-200"
          loading="lazy"
        />
      </div>
    ))}
  </div>
);

export default PostsTab;