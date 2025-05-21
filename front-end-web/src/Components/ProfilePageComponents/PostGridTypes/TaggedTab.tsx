import React from "react";

const dummyTagged = Array.from({ length: 8 }, (_, i) =>
  `https://picsum.photos/seed/tagged${i}/600/600`
);

const TaggedTab: React.FC = () => (
  <div className="grid grid-cols-4 gap-1">
    {dummyTagged.map((src, index) => (
      <div key={index} className="aspect-square overflow-hidden rounded-md border border-gray-200">
        <img
          src={src}
          alt={`tagged-${index}`}
          className="object-cover w-full h-full"
          loading="lazy"
        />
      </div>
    ))}
  </div>
);

export default TaggedTab;