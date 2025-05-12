import { Link } from 'react-router-dom';
import homeIcon from '../../Assets/SideMenu/homeShapeActive.png';
import searchIcon from '../../Assets/SideMenu/magnifyingGlass.png';
import likesIcon from '../../Assets/Post/likeAsStar.png';
import messagesIcon from '../../Assets/SideMenu/chat.png';
import profileIcon from "../../Assets/default_profile_picture.jpg";
import './SideMenu.css';

const SideMenu = () => {
  return (
    <div className="sideMenu">
      <ul className="menuItems">
        <li>
          <Link to="/">
            <img src={homeIcon} alt="Головна" className="menuIcon" />
            Головна
          </Link>
        </li>
        <li>
          <Link to="/search">
            <img src={searchIcon} alt="Пошук" className="menuIcon" />
            Пошук
          </Link>
        </li>
        <li>
          <Link to="/likes">
            <img src={likesIcon} alt="Вподобання" className="menuIcon" />
            Вподобання
          </Link>
        </li>
        <li>
          <Link to="/messages">
            <img src={messagesIcon} alt="Повідомлення" className="menuIcon" />
            Повідомлення
          </Link>
        </li>
        <li>
          <Link to="/profile">
            <img src={profileIcon} alt="Профіль" className="menuIcon" />
            Профіль
          </Link>
        </li>
      </ul>
    </div>
  );
};

export default SideMenu;