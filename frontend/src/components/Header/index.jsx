import '/Header.css';
import { Link } from 'react-router-dom';
import logoImg from '../../assets/images/logo.png';

return (
  <header className="header">
    <img className="logo" src={logoImg} alt="Logo" />
    <nav className="nav">
        <Link to="/">Home</Link>
        <Link to="/about">About</Link>
        <Link to="/contact">Contact</Link>
    </nav>
  </header>
)

export default Header;