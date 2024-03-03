import React from "react";
import Hero from "../../Components/Hero/Hero";

interface Props {
};

const HomePage : React.FC<Props> = ({}: Props) : JSX.Element => {
    return (
        <div>
            <Hero />
        </div>
    )
}

export default HomePage;