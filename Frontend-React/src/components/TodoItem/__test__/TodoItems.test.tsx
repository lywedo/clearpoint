import { render, screen, fireEvent} from '@testing-library/react'
import '@testing-library/jest-dom/extend-expect'
import { TodoItems } from '../TodoItems'
import { act } from 'react-dom/test-utils'

import { useFetchTodoItems, usePutTodoItem } from '../../../hooks'
import React from 'react'


jest.mock('react', () => {
  const actualReact = jest.requireActual('react')
  return {
    ...actualReact,
    useState: jest.fn(),
  }
})
jest.mock('../../../hooks', () => ({
  useFetchTodoItems: jest.fn(),
  usePutTodoItem: jest.fn(),
}))

describe('TodoItems', () => {
  test('renders the todo items correctly', () => {
    const mockTodoItems = [
      { id: '1', description: 'Task 1', isCompleted: false },
      { id: '2', description: 'Task 2', isCompleted: false },
    ]
    const mockFetchData = jest.fn()
    const mockPutData = jest.fn()
    const useStateMock = (init) => [mockTodoItems, jest.fn()]

    useFetchTodoItems.mockReturnValue({
      data: mockTodoItems,
      loading: false,
      error: null,
      fetchData: mockFetchData,
    })

    usePutTodoItem.mockReturnValue({
      data: '1',
      error: null,
      putData: mockPutData,
    })

    jest.spyOn(React, 'useState').mockImplementation(useStateMock)
    render(<TodoItems />)

    expect(screen.getByText('Showing 2 Item(s)')).toBeInTheDocument()
    expect(screen.getByText('Task 1')).toBeInTheDocument()
    expect(screen.getByText('Task 2')).toBeInTheDocument()
  })

test('calls putData when "Mark as completed" button is clicked', async () => {
    const mockTodoItems = [
      { id: '1', description: 'Task 1', isCompleted: false },
      { id: '2', description: 'Task 2', isCompleted: false },
    ];
    const mockFetchData = jest.fn();
    const mockPutData = jest.fn();
    const useStateMock = (init) => [mockTodoItems, jest.fn()];
  
    useFetchTodoItems.mockReturnValue({
      data: mockTodoItems,
      loading: false,
      error: null,
      fetchData: mockFetchData,
    });
  
    usePutTodoItem.mockReturnValue({
      data: '1',
      error: null,
      putData: mockPutData,
    });
  
    jest.spyOn(React, 'useState').mockImplementation(useStateMock);
  
    render(<TodoItems />);
  
    const markAsCompletedButtons = screen.getAllByText('Mark as completed');
    fireEvent.click(markAsCompletedButtons[0]);
    render(<TodoItems />);
    await act(async () => {
      expect(screen.getByText('completed')).toBeInTheDocument();
    });
  })
})
