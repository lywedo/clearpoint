import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import { TodoItems } from '../TodoItems';

jest.mock('../../hooks', () => ({
  __esModule: true,
  useFetchTodoItems: jest.fn(),
  usePutTodoItem: jest.fn(),
}));

describe('TodoItems component', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  test('renders the component', () => {
    render(<TodoItems />);
    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  test('fetches data and renders todo items', async () => {
    const mockItems = [{ id: '1', description: 'Test item', isCompleted: false }];
    const mockFetchData = jest.fn().mockResolvedValue(mockItems);
    const mockPutData = jest.fn();
    const mockUseFetchTodoItems = jest.fn().mockReturnValue({ data: mockItems, loading: false, error: null, fetchData: mockFetchData });
    const mockUsePutTodoItem = jest.fn().mockReturnValue({ data: null, error: null, putData: mockPutData });

    jest.doMock('../../hooks', () => ({
      __esModule: true,
      useFetchTodoItems: mockUseFetchTodoItems,
      usePutTodoItem: mockUsePutTodoItem,
    }));

    render(<TodoItems />);
    await waitFor(() => {
      expect(mockFetchData).toHaveBeenCalled();
      expect(screen.getByText('Test item')).toBeInTheDocument();
    });
  });

  test('handles marking item as complete', async () => {
    const mockItems = [{ id: '1', description: 'Test item', isCompleted: false }];
    const mockFetchData = jest.fn().mockResolvedValue(mockItems);
    const mockPutData = jest.fn().mockResolvedValue(null);
    const mockUseFetchTodoItems = jest.fn().mockReturnValue({ data: mockItems, loading: false, error: null, fetchData: mockFetchData });
    const mockUsePutTodoItem = jest.fn().mockReturnValue({ data: null, error: null, putData: mockPutData });

    jest.doMock('../../hooks', () => ({
      __esModule: true,
      useFetchTodoItems: mockUseFetchTodoItems,
      usePutTodoItem: mockUsePutTodoItem,
    }));

    render(<TodoItems />);
    await waitFor(() => {
      const markAsCompleteBtn = screen.getByText('Mark as completed');
      fireEvent.click(markAsCompleteBtn);
      expect(mockPutData).toHaveBeenCalledWith({ id: '1', description: 'Test item', isCompleted: true }, '1');
    });
  });
});
