import { Navigate } from "react-router-dom";
import { toast } from "react-toastify";
import { useRef } from "react";

const AdminRoute = ({ children }) => {
    const role = localStorage.getItem("role");
    const hasShownToast = useRef(false);

    if (role !== "Admin") {
        if (!hasShownToast.current) {
            toast.error("Only available for admins.");
            hasShownToast.current = true;
        }
        return <Navigate to ="/trainings" replace />;
    }

    return children;
}

export default AdminRoute;