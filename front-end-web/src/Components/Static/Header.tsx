export default function Header() {
    return (<div>
        <header className="bg-gray-800 text-white py-4">
            <div className="container mx-auto flex justify-between items-center">
                <h1 className="text-2xl font-bold">Social Media App</h1>
                <nav>
                    <ul className="flex space-x-4">
                        <li><a href="/" className="hover:text-gray-400">Home</a></li>
                        <li><a href="/about" className="hover:text-gray-400">About</a></li>
                        <li><a href="/contact" className="hover:text-gray-400">Contact</a></li>
                    </ul>
                </nav>
            </div>
        </header>
    </div>)
}