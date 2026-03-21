import './Home.css'  
import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getAllPosts } from '../../services/posts';
import Header from '../../components/Header';

return(
    <div className="home">
        <Header />   
    </div>
)

export default Home;