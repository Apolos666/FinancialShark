import axios from "axios";
import {toast} from "react-toastify";

export const handleError = (error: any) => {
    if (axios.isAxiosError(error))
    {
        var errorResponse = error.response;

        if (Array.isArray(errorResponse?.data.errors))
        {
            for (let val of errorResponse?.data.errors)
                toast.warning(val.description);
        } else if (typeof errorResponse?.data.errors === "object")
        {
            for (let e in errorResponse?.data.errors)
                toast.warning(errorResponse?.data.errors[e][0]);
        } else if (errorResponse?.data)
        {
            toast.warning(errorResponse?.data);
        } else if (errorResponse?.status === 401)
        {
            toast.warning("Please login to continue");
            window.history.pushState({}, "LoginPage", "/login");
        } else if (errorResponse)
        {
            toast.warning(errorResponse?.data);
        }
    }

}
