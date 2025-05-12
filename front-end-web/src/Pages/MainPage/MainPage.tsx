import SideMenu from "../../Components/SideMenu/SideMenu";
import PostCard from "../../Components/PostCard/PostCard";
import GloweeLogo from "../../Assets/Glowee Logo.png";
import defaultImage from "../../Assets/default_profile_picture.jpg"
import PostImage1 from "../../Assets/Post/Photo1.png";
import PostImage2 from "../../Assets/Post/Photo2.png";
import "./MainPage.css";

export default function MainPage() {
  return (
    <div className="mainPageWrapper">
      <SideMenu />
      <div className="centerFeed">
        <img src={GloweeLogo} alt="" className="Logo" />
         <PostCard
            username="glowee_dev"
            userImage={defaultImage}
            postImages={[defaultImage, PostImage1, PostImage2]}
            caption="Це мій перший пост у нашому Instagram-клоні!"
          />
          <PostCard
            username="glowee_dev"
            userImage={defaultImage}
            postImages={[defaultImage, PostImage1, PostImage2]}
            caption="Другий пост у нашому Instagram-клоні!"
          />
      </div>
      <div className="rightSidebar">Привіт з правого сайдбару</div>
    </div>
  );
}