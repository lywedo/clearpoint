import { useState, useCallback } from 'react';
import axios, { AxiosRequestConfig } from 'axios';

interface PostState<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
  postData: (body: any) => void;
  cleanData: () => void;
}

const usePostTodoItem = <T>(endpoint: string, config?: AxiosRequestConfig): PostState<T> => {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const url = `${process.env.REACT_APP_API_URL}/${endpoint}`;

  const postData = useCallback(async (body: any) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.post<T>(url, body, config);
      setData(response.data);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        setError(err.response?.data);
      } else {
        setError('An unexpected error occurred');
      }
    } finally {
      setLoading(false);
    }
  }, [url, config]);

  const cleanData = ()=>{
    setData(null);
    setError(null);
  }

  return { data, loading, error, postData, cleanData };
};

export default usePostTodoItem;
