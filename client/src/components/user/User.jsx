import { useEffect, useState } from "react"
import UserForm from "./UserForm"
import UserList from "./UserList"
import { useForm } from "react-hook-form"
import toast from "react-hot-toast";
import axios from "axios";

function User() {

  const BASE_URL = import.meta.env.VITE_BASE_API_URL;

  const [usersList, setUserList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [editData, setEditData] = useState(null);

  useEffect(() => {
    const loadUsers = async () => {
      try {
        const users = (await axios.get(BASE_URL)).data;
        setUserList(users);
      } catch (error) {
        console.log(error);
        toast.error("An error has occured.");
      } finally {
        setLoading(false);
      }
    };

    loadUsers();
  }, [])

  useEffect(() => {
    methods.reset(editData);
  }, [editData])

  const defaultFormValues = {
    id: 0,
    firstName: '',
    lastName: ''
  }

  const methods = useForm({
    defaultValues: defaultFormValues
  });


  const handleFormSubmit = async (user) => {
    try {
      setLoading(true);
      if (user.id <= 0) {
        const createdUser = (await axios.post(BASE_URL, user)).data;
        setUserList((previousUser) => [...previousUser, createdUser]);
      }
      else {
        await axios.put(`${BASE_URL}/${user.id}`, user);
        setUserList((previousUser) => previousUser.map(p => p.id === user.id ? user : p));
      }

      methods.reset(defaultFormValues);
      toast.success("Saved Successfully");
    } catch (error) {
      console.log(error);
      toast.error("An error has occured.");
    } finally {
      setLoading(false);
    }
  }

  const handleUserEdit = (user) => {
    setEditData(user);
  }

  const handleUserDelete = async (user) => {
    if (!confirm(`Are you sure you want to delete user: ${user.firstName} ${user.lastName}?`)) {
      return;
    }
    setLoading(true);
    try {
      await axios.delete(`${BASE_URL}/${user.id}`);
      setUserList((previousUser) => previousUser.filter(p => p.id !== user.id))
      toast.success("Deleted Successfully");
    } catch (error) {
      toast.error("Error on deleting.");
    } finally {
      setLoading(false);
    }
  }

  const handleFormReset = () => {
    methods.reset(defaultFormValues);
  }

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 space-y-6">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
            User Management
          </h1>
          {loading && <p>Loading...</p>}
        </div>

        <UserForm methods={methods} onFormReset={handleFormReset} onFormSubmit={handleFormSubmit} />
        <UserList usersList={usersList} onUserEdit={handleUserEdit} onUserDelete={handleUserDelete} />
      </div>
    </div>
  )
}

export default User