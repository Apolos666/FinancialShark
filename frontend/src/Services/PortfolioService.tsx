import axios from "axios";
import {handleError} from "../Helpers/ErrorHandler";
import {PortfolioGet, PortfolioPost} from "../Models/Portfolio";

const api = "http://localhost:5034/api/portfolio/";

export const portfolioAddAPI = async (symbol: string) => {
    try {
        const data = await axios.post<PortfolioPost>(api + `?symbol=${symbol}`);
        return data;
    } catch (e)
    {
        handleError(e);
    }
}

export const portfolioDeleteAPI = async (symbol: string) => {
    try {
        const data = await axios.delete<PortfolioPost>(api + `?symbol=${symbol}`);
        return data;
    } catch (e)
    {
        handleError(e);
    }
}

export const portfolioGetAPI = async () => {
    try {
        const data = await axios.get<PortfolioGet[]>(api);
        return data;
    } catch (e)
    {
        handleError(e);
    }
}