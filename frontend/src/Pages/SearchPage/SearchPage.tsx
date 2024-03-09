import React, {ChangeEvent, SyntheticEvent, useEffect, useState} from "react";
import Navbar from "../../Components/Navbar/Navbar";
import Search from "../../Components/Search/Search";
import ListPortfolio from "../../Components/Portfolio/ListPortfolio/ListPortfolio";
import CardList from "../../Components/CardList/CardList";
import {CompanySearch} from "../../company";
import {searchCompanies} from "../../api";
import {PortfolioGet} from "../../Models/Portfolio";
import {portfolioAddAPI, portfolioDeleteAPI, portfolioGetAPI} from "../../Services/PortfolioService";
import {toast} from "react-toastify";

interface Props {
};

const SearchPage : React.FC<Props> = ({}: Props) : JSX.Element => {

    const [search, setSearch] = useState<string>("");
    const [portfolioValues, setPortfolioValues] = useState<PortfolioGet[] | null>([]);
    const [searchResult, setSearchResult] = useState<CompanySearch[]>([]);
    const [serverError, setServerError] = useState<string | null>(null);

    useEffect(() => {
        getPortfolio();
    }, []);

    const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
        setSearch(e.target.value);
        console.log(e);
    };

    const onSearchSubmit = async (e: SyntheticEvent) => {
        e.preventDefault();

        const result = await searchCompanies(search);

        if (typeof result === "string") {
            setServerError(result);
        } else if (Array.isArray(result.data)) {
            setSearchResult(result.data);
        }

        console.log(searchResult);
    };

    const onPortfolioCreate = (e: any) => {
        e.preventDefault();

        portfolioAddAPI(e.target[0].value)
            .then((res) => {
                if (res?.status === 204) {
                    toast.success("Added to portfolio");
                    getPortfolio();
                }
            }).catch((e) => {
                toast.warning("Could not add to portfolio");
        })
    }

    const onPortfolioDelete = (e: any) => {
        e.preventDefault();

        portfolioDeleteAPI(e.target[0].value)
            .then((res) => {
                if (res?.status === 200) {
                    toast.success("Removed from portfolio");
                    getPortfolio();
                }
            }).catch((e) => {
                toast.warning("Could not remove from portfolio");
        })
    }

    const getPortfolio = async () => {
        portfolioGetAPI()
            .then((res) => {
                if (res) {
                    setPortfolioValues(res?.data);
                }
            }).catch((e) => {
            toast.warning("Could not get portfolio");
        })
    }


    return (
        <div className="App">
            <Search
                search={search}
                handleSearchChange={handleSearchChange}
                onSearchSubmit={onSearchSubmit}
            />
            <ListPortfolio
                portfolioValues={portfolioValues!}
                onPortfolioDelete={onPortfolioDelete}/>
            <CardList
                searchResults={searchResult}
                onPortfolioCreate={onPortfolioCreate}
            />
            {serverError && <h1>{serverError}</h1>}
        </div>
    )
}

export default SearchPage;